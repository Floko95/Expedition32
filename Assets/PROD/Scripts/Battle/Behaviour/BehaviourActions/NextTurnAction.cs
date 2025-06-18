using System;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "NextTurn", story: "[BattleManager] Computes the next [BattleState] and [Unit] turn.", category: "Action", id: "26837cc04b22d3da026e1643be4c8235")]
public partial class NextTurnAction : Action
{
    [SerializeReference] public BlackboardVariable<BattleManager> BattleManager;
    [SerializeReference] public BlackboardVariable<BattleState> BattleState;
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    
    
    protected override Status OnStart() {
        if (HasBattleEnded()) {
            BattleState.Value = IsBattleWon() ? global::BattleState.Win : global::BattleState.Loss;
            return Status.Success;
        } else {
            
            Unit.Value = BattleManager.Value.TurnQueue.GetNext();
            
            BattleState.Value = Unit.Value is AllyUnit ? global::BattleState.PlayerTurn : global::BattleState.EnemyTurn;
            global::BattleManager.onTurnStarted.Invoke(Unit.Value);
        }
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
        
    }

    private bool HasBattleEnded() {
        return BattleManager.Value.Battle.Allies.All(u => !u.IsAlive) 
            || BattleManager.Value.Battle.Enemies.All(u => !u.IsAlive);
    }

    private bool IsBattleWon() {
        return BattleManager.Value.Battle.Enemies.All(u => !u.IsAlive);
    }
}

