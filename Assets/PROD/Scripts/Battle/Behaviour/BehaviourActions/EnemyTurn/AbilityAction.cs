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
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<AbilityData> Ability;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Targets;

    private bool _hasCinematicEnded;
    private IDisposable _abilityExecution;
    
    protected override Status OnStart() {
        _hasCinematicEnded = false;
        BattleLogDebugUI.Log(Ability.Value.desc);

        _hasCinematicEnded = false;
        var battleManager = Toolbox.Get<BattleManager>();
        _abilityExecution = battleManager.ExecuteAbility(Unit.Value, Targets.Value.Select(u => u.GetComponent<Unit>()).ToList(), Ability.Value).Subscribe(
            onNext: _ => { },
            onCompleted: _ => _hasCinematicEnded = true
        );
        
        return Status.Running;
        
    }

    protected override Status OnUpdate() {
        return _hasCinematicEnded ? Status.Success : Status.Running;
    }

    protected override void OnEnd() {
        _abilityExecution.Dispose();
    }
}

