using System;
using System.Collections.Generic;
using System.Linq;
using BitDuc.Demo;
using BitDuc.EnhancedTimeline.Timeline;
using R3;
using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayerAbilityAction", story: "[Unit] Uses [AbilityData] on [Targets]", category: "Action",
    id: "3bec253d7a76de6e6a983cf2ba3ab50a")]
public partial class PlayerAbilityAction : Action {
    
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<AbilityData> AbilityData;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Targets;

    private bool _hasCinematicEnded;
    private CinemachineVirtualCameraBase _vCam;
    private IDisposable _comboListener;
    
    protected override Status OnStart() {
        if (Unit.Value == null)
            Debug.Log("No Unit selected");
        if (AbilityData.Value == null)
            Debug.Log("No Ability selected");

        if (Unit.Value.APSystem.CanSpendAP(AbilityData.Value.costAP) == false) return Status.Failure;
        
        _hasCinematicEnded = false;
        BattleLogDebugUI.Log($"Character attacks {Targets.Value[0].name}");
        
        var player = Unit.Value.playableDirector;
        ManageBindings(player); //Manual Bindings HERE , move elsewhere next

        _comboListener = player.Listen<ComboWindow>()
            .Subscribe(HandleComboWindow)
            .AddTo(Unit.Value.gameObject);
        
        player.Play(AbilityData.Value.timeline).Subscribe(
            onNext: _ => OnTimelineUpdate(),
            onCompleted: _ => OnTimelineComplete()
        );

        return Status.Running;
    }

    private void OnTimelineUpdate() {

    }

    private void OnTimelineComplete() {
        _hasCinematicEnded = true;
        _comboListener.Dispose();
    }

    private void ManageBindings(EnhancedTimelinePlayer playableDirector) {
        _vCam = Unit.Value.transform.Find(AbilityAction.ABILITY_CAM_NAME)?.GetComponent<CinemachineVirtualCameraBase>();

        if (_vCam != null) {
            _vCam.Priority = 100;
        }
    }

    protected override Status OnUpdate() {
        return _hasCinematicEnded ? Status.Success : Status.Running;
    }

    protected override void OnEnd() {
        if (_vCam != null) {
            _vCam.Priority = 0;
        }
    }
    
    private void HandleComboWindow(ComboWindow window) {
        if(AbilityData.Value == null) return;

        foreach (var target in Targets.Value.Select(t => t.GetComponent<Unit>()).Where(u => u != null)) {
            AbilityData.Value.ApplyEffects(Unit.Value, target);
        }
    }
}

