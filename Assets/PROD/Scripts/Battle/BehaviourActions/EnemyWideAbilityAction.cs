using System;
using BitDuc.Demo;
using BitDuc.EnhancedTimeline.Timeline;
using R3;
using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyWideAbilityAction", story: "[Unit] attacks the expedition with [Ability]", category: "Action", id: "63aade254f79a7d8ba433e7c642a90c2")]
public partial class EnemyWideAbilityAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<AbilityData> Ability;
    [SerializeReference] public BlackboardVariable<BattleManager> BattleManager;
    
    
    private bool _hasCinematicEnded;
    private CinemachineVirtualCameraBase _vCam;
    private IDisposable _comboListener;
    
    protected override Status OnStart()
    {
        if(Unit.Value == null)
            Debug.LogError("No Unit selected");
        if(Ability.Value == null)
            Debug.LogError("No Ability selected");
        
        BattleLogDebugUI.Log(Ability.Value.desc);
        
        var player = Unit.Value.playableDirector;
        ManageBindings(player); //Manual Bindings HERE , move elsewhere next
            
        _comboListener = player.Listen<ComboWindow>()
            .Subscribe(HandleComboWindow)
            .AddTo(Unit.Value.gameObject);
        
        player.Play(Ability.Value.timeline).Subscribe(
            onNext: _ => OnTimelineUpdate(),
            onCompleted: _ => OnTimelineComplete()
        );

        
        return Status.Running;
    }

    private void HandleComboWindow(ComboWindow window) {
        if(Ability.Value == null) return;
        
        foreach (var unit in BattleManager.Value.Battle.Allies) {
            //TODO ADD PARRY CHECK, APPLY EFFECTS OF ABILITY INSTEAD
            Ability.Value.ApplyEffects(Unit.Value, unit);
        }
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

    protected override void OnEnd()
    {
        
    }
}

