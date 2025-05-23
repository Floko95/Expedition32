using System;
using BitDuc.EnhancedTimeline.Timeline;
using R3;
using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Ability", story: "[Unit] uses [ability] on a single [Target]", category: "Action", id: "285316aaf1c489624db0457ded13ddb2")]
public partial class AbilityAction : Action
{
    private static string ABILITY_CAM_NAME = "AbilityVCam";
    
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<AbilityData> Ability;
    [SerializeReference] public BlackboardVariable<Unit> Target;
    
    private bool _hasCinematicEnded;
    private CinemachineVirtualCameraBase _vCam;
    
    //Run to target, then play cinematic. For now we only play cinematic
    
    protected override Status OnStart() {
        if(Unit.Value == null)
            Debug.Log("No Unit selected");
        if(Ability.Value == null)
            Debug.Log("No Ability selected");
        
        _hasCinematicEnded = false;
        BattleLogDebugUI.Log($"{Unit.Value.unitData.unitName} uses {Ability.Value.title} on {Target?.Value?.unitData.name} !");
        
        
        
        var player = Unit.Value.playableDirector;
        
        //Manual Bindings HERE , move elsewhere next
        ManageBindings(player);
            
        
        player.Play(Ability.Value.timeline).Subscribe(
            onNext: _ => OnTimelineUpdate(),
            onCompleted: _ => _hasCinematicEnded = true
        );
        
        return Status.Running;
    }

    private void ManageBindings(EnhancedTimelinePlayer playableDirector) {
        //playableDirector.bindings.Add(Camera.main.gameObject);
        
        _vCam = Unit.Value.transform.Find(ABILITY_CAM_NAME)?.GetComponent<CinemachineVirtualCameraBase>();
        if (_vCam != null) {
            _vCam.Priority = 100;
        }
    }
    
    private void OnTimelineUpdate() { }

    protected override Status OnUpdate() {
        return _hasCinematicEnded ? Status.Success : Status.Running;
    }

    protected override void OnEnd() {
        if (_vCam != null) {
            _vCam.Priority = 0;
        }
    }
}

