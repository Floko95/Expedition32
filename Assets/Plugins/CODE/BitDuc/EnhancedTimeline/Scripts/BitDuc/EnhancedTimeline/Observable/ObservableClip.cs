using R3;
using UnityEngine.Playables;

namespace BitDuc.EnhancedTimeline.Observable
{
    /**
     * Observable clip on an observable track. Emitted as an event from the
     * @ref BitDuc.EnhancedTimeline.Timeline.EnhancedTimelinePlayer "EnhancedTimelinePlayer" and
     * @ref BitDuc.EnhancedTimeline.Timeline.TimelineBus "TimelineBus" components.
     */
    public abstract class ObservableClip : PlayableAsset, TimelineEvent {
        
        public double start, end, clipDuration;

        /**
         * Called when the clip is started.
         * @param frames An observable that starts, updates, and completes throughout the clip playing. 
         */
        public abstract void StartClip(Observable<FrameData> frames);
    }
}
