using UnityEngine;

public sealed class BattleStateMachine : StateMachine<ABattleState>
{
    private PlayerTurnState _playerTurnState;
    
    public BattleStateMachine(BattleManager battleManager) {
        AddState(new PlayerTurnState(battleManager, Toolbox.Get<TargetManager>()));
    }
}
