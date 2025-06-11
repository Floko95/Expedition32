using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Unity.Behavior;
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
    private bool _isCounterAvailable;
    private IDisposable _abilityExecution;
    
    protected override Status OnStart() {
        _hasCinematicEnded = false;
        BattleLogUI.Log(Ability.Value.desc);

        _hasCinematicEnded = false;
        var battleManager = Toolbox.Get<BattleManager>();
        
        _isCounterAvailable = true;
        _abilityExecution = battleManager.ExecuteAbility(Unit.Value, Targets.Value.Select(u => u.GetComponent<Unit>()).ToList(), Ability.Value).Subscribe(
            onNext: _ => { },
            onCompleted: _ => {
                if (_isCounterAvailable && Ability.Value.targetMode is AbilityTargetMode.SelectTarget) Counter();
                else _hasCinematicEnded = true;
            }
        );
        
        return Status.Running;
    }

    protected override Status OnUpdate() {
        return _hasCinematicEnded ? Status.Success : Status.Running;
    }

    protected override void OnEnd() {
        _abilityExecution?.Dispose();
    }
    
    private void Counter() {
        var battleManager = Toolbox.Get<BattleManager>();
        var singleTarget = Targets.Value[0].GetComponent<Unit>() as AllyUnit;
        singleTarget.DodgeSystem.enabled = false;
            
        BattleLogUI.Log(singleTarget.unitData.counterAbility.desc);
        _abilityExecution = battleManager.ExecuteAbility(singleTarget, new List<Unit> {Unit.Value}, singleTarget.unitData.counterAbility).Subscribe(
            onNext: _ => { },
            onCompleted: _ => { _hasCinematicEnded = true; }
        );
    }
}

