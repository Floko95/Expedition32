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
[NodeDescription(name: "Ability", story: "[Unit] uses [ability] on [Targets]", category: "Action",
    id: "285316aaf1c489624db0457ded13ddb2")]
public partial class AbilityAction : Action {
    
    public static string ABILITY_CAM_NAME = "AbilityVCam";

    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<AbilityData> Ability;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Targets;

    private bool _hasCinematicEnded;
    
    private CinemachineVirtualCameraBase _vCam;
    private IDisposable _comboListener;
    private List<Unit> _targets;
    
    protected override Status OnStart() {
        if (Unit.Value == null)
            Debug.Log("No Unit selected");
        if (Ability.Value == null)
            Debug.Log("No Ability selected");
        
        _hasCinematicEnded = false;
        BattleLogDebugUI.Log(Ability.Value.desc);

        var player = Unit.Value.playableDirector;
        ManageBindings(player);

        _targets = Targets.Value.Select(u => u.GetComponent<Unit>()).Where(u => u != null).ToList();
        _comboListener = player.Listen<ComboWindow>()
            .Subscribe(HandleComboWindow)
            .AddTo(Unit.Value.gameObject);

        player.Play(Ability.Value.timeline).Subscribe(
            onNext: _ => OnTimelineUpdate(),
            onCompleted: _ => OnTimelineComplete()
        );
        
        return Status.Running;
    }
    
    private void ManageBindings(EnhancedTimelinePlayer playableDirector) {
        _vCam = Unit.Value.transform.Find(ABILITY_CAM_NAME)?.GetComponent<CinemachineVirtualCameraBase>();

        if (_vCam != null) {
            _vCam.Priority = 100;
        }
    }

    private void OnTimelineUpdate() { }

    private void OnTimelineComplete() {
        _hasCinematicEnded = true;
        _comboListener.Dispose();
    }

    private void HandleComboWindow(ComboWindow window) {
        if (Ability.Value == null) return;
        
        foreach (var target in _targets) {
            if(target is AllyUnit allyUnit)
                BattleLogic.TryApplyAbilityEffects(Ability.Value, Unit.Value, allyUnit);
            else
                Ability.Value.ApplyEffects(Unit.Value, target);
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
}

