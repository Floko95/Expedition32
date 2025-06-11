using System;
using System.Collections.Generic;
using System.Linq;
using BitDuc.Demo;
using R3;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Serialization;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CounterAction", story: "[Counterer] counters.", category: "Action", id: "031a50f6f20ab5de752a0cf82c69dd8a")]
public partial class CounterAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Attacker;
    [SerializeReference] public BlackboardVariable<Unit> Counterer;
    [SerializeReference] public BlackboardVariable<AbilityData> CounterAbility;
    
    private bool _hasCinematicEnded;
    private IDisposable _abilityExecution;
    
    protected override Status OnStart() {
        BattleLogUI.Log(CounterAbility.Value.desc);

        _hasCinematicEnded = false;
        
        var battleManager = Toolbox.Get<BattleManager>();
        _abilityExecution = battleManager.ExecuteAbility(Counterer.Value, new List<Unit> {Attacker}, CounterAbility.Value).Subscribe(
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

