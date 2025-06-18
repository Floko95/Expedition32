using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Unity.Behavior;
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
    private IDisposable _abilityExecution;
    
    protected override Status OnStart() {
        if (Unit.Value.APSystem.CanSpendAP(AbilityData.Value.costAP) == false) return Status.Failure;
        
        _hasCinematicEnded = false;
        var battleManager = Toolbox.Get<BattleManager>();
        var target = Targets.Value.Select(u => u.GetComponent<Unit>()).ToList();
        var observable =  battleManager.ExecuteAbility(Unit.Value, target, AbilityData.Value);
        
        if (observable == null) { //no cinematic
            _hasCinematicEnded = true;
            return Status.Running;
        }
        
        _abilityExecution = observable.Subscribe(
            onNext: _ => { },
            onCompleted: _ => _hasCinematicEnded = true
        );
        
        return Status.Running;
    }

    protected override Status OnUpdate() {
        return _hasCinematicEnded ? Status.Success : Status.Running;
    }

    protected override void OnEnd() {
        Unit.Value.OnAbilityUsed?.Invoke(AbilityData.Value);
        _abilityExecution?.Dispose();
    }
}

