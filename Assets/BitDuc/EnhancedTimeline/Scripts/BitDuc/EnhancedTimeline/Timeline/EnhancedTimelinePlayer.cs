using System;
using System.Collections.Generic;
using System.Linq;
using BitDuc.EnhancedTimeline.Observable;
using BitDuc.EnhancedTimeline.Utilities;
using R3;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace BitDuc.EnhancedTimeline.Timeline
{
    /**
     * A timeline player that can play, pause, and stop playable timelines from
     * @ref BitDuc.EnhancedTimeline.PlayableSerialization.PlayableReference "PlayableReference" and **PlayableAsset**
     * objects.
     * It can play multiple timelines concurrently. It features simple bindings from objects to tracks, allowing simpler
     * handling of several timelines. Allows listening to events from a single timeline being played, or all on the same
     * observable.
     */
    public class EnhancedTimelinePlayer : MonoBehaviour, TimelinePlayer
    {
        public IEnumerable<PlayableAsset> CurrentlyPlaying =>
            currentlyPlaying.Select(director => director.playableAsset);

        [Serializable]
        public struct TrackBinding
        {
            public string trackName;
            public GameObject bound;
        }

        /**
         * Controls how the timeline updates every frame. It is equivalent to the **PlayableDirector.updateMethod**.
         */
        public DirectorUpdateMode updateMethod = DirectorUpdateMode.GameTime;

        /**
         * **GameObjects** used to search for **Components** to bind timeline tracks to.
         */
        public List<GameObject> bindings;

        /**
         * NamedBindings used to bind the named track to a component on the corresponding **GameObject**.
         */
        public List<TrackBinding> trackBindings;

        [Header("Debug")]
        [SerializeField]
        bool focusPlayableDirector = true;

        readonly Subject<TimelineEvent> events = new();
        readonly HashSet<PlayableDirector> currentlyPlaying = new();

        Pool players;

        void Awake()
        {
            var prototype = BuildDirectorPrototype();
            players = new Pool(prototype, transform);
        }

        public Observable<TimelineEvent> Play(PlayableAsset timeline)
        {
            var (player, bus) = GetPlayer();
            Bind(player, timeline, bus);

            var onStop = Play(timeline, player);
            AddToCurrentlyPlaying(player, onStop);

            var timelineEvents = EmitEventsUntil(bus, onStop).Share();
            timelineEvents.Subscribe();

#if UNITY_EDITOR
            if (focusPlayableDirector)
                SeekBarCursorFix.Play(
                    gameObject,
                    player.gameObject,
                    timelineEvents.AsUnitObservable()
                );
#endif

            return timelineEvents;
        }

        public void Pause(PlayableAsset timeline)
        {
            var directors = currentlyPlaying
                .Where(director => director.playableAsset == timeline);

            foreach (var director in directors)
                director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }

        public void Resume(PlayableAsset timeline)
        {
            var directors = currentlyPlaying
                .Where(director => director.playableAsset == timeline);

            foreach (var director in directors)
                director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

        public void PauseAll()
        {
            foreach (var director in currentlyPlaying)
                director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }

        public void ResumeAll()
        {
            foreach (var director in currentlyPlaying)
                director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

        public void Stop(PlayableAsset timeline)
        { 
            var directors = currentlyPlaying
                .Where(director => director.playableAsset == timeline)
                .ToArray();

            foreach(var director in directors)
                director.Stop();
        }

        public Observable<T> Listen<T>() where T: TimelineEvent =>
            events.Where(message => message is T)
                .Select(message => (T)message);

        Observable<PlayableDirector> Play(PlayableAsset timeline, PlayableDirector player)
        {
            player.Play(timeline);
            return OnPlayableDirectorStopped(player).Take(1).Share();
        }

        (PlayableDirector, TimelineBus) GetPlayer()
        {
            var player = players.Get();
            player.hideFlags = HideFlags.DontSave;
            return (player.GetComponent<PlayableDirector>(), player.GetComponent<TimelineBus>());
        }

        void AddToCurrentlyPlaying(PlayableDirector player, Observable<PlayableDirector> onStop)
        {
            currentlyPlaying.Add(player);
            onStop.Subscribe(_ => {
                currentlyPlaying.Remove(player);
                players.Release(player.gameObject);
            });
        }

        Observable<TimelineEvent> EmitEventsUntil(TimelineBus timelineEvents, Observable<PlayableDirector> onStop) =>
            timelineEvents
                .Listen<TimelineEvent>()
                .Do(PublishToBus)
                .TakeUntil(onStop)
                .DefaultIfEmpty();

        void PublishToBus(TimelineEvent @event) =>
            events.OnNext(@event);

        void Bind(PlayableDirector player, PlayableAsset timeline, TimelineBus bus)
        {
            foreach (var output in timeline.outputs)
            {
                var binding =
                    output.outputTargetType == typeof(GameObject) ? FindGameObjectBinding(output) :
                    output.outputTargetType == typeof(TimelineBus) ? bus :
                    FindComponentBinding(output);

                player.SetGenericBinding(output.sourceObject, binding);
            }
        }

        GameObject FindGameObjectBinding(PlayableBinding output) =>
            FindNamedGameObjectBinding(output) ??
            bindings.FirstOrDefault();

        GameObject FindNamedGameObjectBinding(PlayableBinding output) =>
            trackBindings
                .Select(binding => (TrackBinding?)binding)
                .FirstOrDefault(binding => binding?.trackName == output.streamName)
                ?.bound;

        Object FindComponentBinding(PlayableBinding output) =>
            FindNamedComponentBinding(output) ??
            FindUnnamedComponentBinding(output);

        Object FindNamedComponentBinding(PlayableBinding output) =>
            FindNamedGameObjectBinding(output)
                ?.GetComponent(output.outputTargetType);

        Object FindUnnamedComponentBinding(PlayableBinding output) =>
            bindings
                .Select(binding => binding.GetComponent(output.outputTargetType))
                .FirstOrDefault(component => component != null);

        Observable<PlayableDirector> OnPlayableDirectorStopped(PlayableDirector player) =>
            R3.Observable.FromEvent<PlayableDirector>(
                handler => player.stopped += handler,
                handler => player.stopped -= handler
            );

        GameObject BuildDirectorPrototype()
        {
            var prototype = new GameObject(
                "EnhancedTimeline - PlayableDirectorPrototype",
                typeof(TimelineBus)
            ) { hideFlags = HideFlags.HideAndDontSave };

            prototype.transform.parent = transform;
            var director = prototype.AddComponent<PlayableDirector>();
            director.timeUpdateMode = updateMethod;
            return prototype;
        }
    }
}
