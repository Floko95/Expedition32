using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Ability", story: "[Unit] uses [ability] on a single [Target]", category: "Action", id: "285316aaf1c489624db0457ded13ddb2")]
public partial class AbilityAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<AbilityData> Ability;
    [SerializeReference] public BlackboardVariable<Unit> Target;
    
    private bool _hasCinematicEnded;
    
    //Run to target, then play cinematic. For now we only play cinematic
    
    protected override Status OnStart() {
        if(Unit.Value == null)
            Debug.Log("No Unit selected");
        if(Ability.Value == null)
            Debug.Log("No Ability selected");
        
        _hasCinematicEnded = false;
        
        var director = Unit.Value.playableDirector;
        director.playableAsset = Ability.Value.timeline;
        BindTimelineTracks(director, Ability.Value.timeline);
        director.Play();
        director.stopped += OnCutscenePlayed;
        
        BattleLogDebugUI.Log($"{Unit.Value.unitData.unitName} uses {Ability.Value.title} on {Target?.Value?.unitData.name} !");
        
        return Status.Running;
    }

    private void OnCutscenePlayed(PlayableDirector obj) {
        _hasCinematicEnded = true;
        Debug.Log("Cinematic Ended");
    }

    protected override Status OnUpdate() {
        return _hasCinematicEnded ? Status.Success : Status.Running;
    }

    protected override void OnEnd() {
        Unit.Value.playableDirector.stopped -= OnCutscenePlayed;
    }

    public void BindTimelineTracks(PlayableDirector director, TimelineAsset timelineAsset) {
        var animator = Unit.Value.gameObject.GetComponentInChildren<Animator>();
        foreach (var track in timelineAsset.GetOutputTracks()) {
            if (track is AnimationTrack) {
                director.SetGenericBinding(track, animator);
            }
        }
    }
}

