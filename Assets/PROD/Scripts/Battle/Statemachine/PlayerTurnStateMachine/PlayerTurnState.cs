using UnityEngine;

public sealed class PlayerTurnStateMachine : StateMachine<APlayerTurnSubState> {
    private DefaultSelectionState _defaultSelectionState;
    private TargetSelectionState _targetSelectionState;
    
    public PlayerTurnStateMachine(PlayerTurnState ctx, TargetManager targetManager) {
        _defaultSelectionState = new DefaultSelectionState(ctx);
        _targetSelectionState = new TargetSelectionState(ctx, targetManager);
        
        AddState(_defaultSelectionState);
        AddState(_targetSelectionState);
    }
}

public abstract class APlayerTurnSubState : IState<APlayerTurnSubState> {
    
    protected PlayerTurnState _context;

    public APlayerTurnSubState(PlayerTurnState playerTurnState) {
        _context = playerTurnState;
    }
    
    public abstract void Enter(APlayerTurnSubState previousState);
    public abstract void Update(float deltaTime);

    public abstract void FixedUpdate(float fixedDeltaTime);

    public abstract void Exit(APlayerTurnSubState nextState);
}

public class PlayerTurnState : ABattleState {

    public AllyUnit CurrentTurnUnit => _battleManager.CurrentTurnUnit as AllyUnit;
    
    protected PlayerTurnStateMachine _playerTurnStateMachine;
    
    public PlayerTurnState (BattleManager battleManager, TargetManager targetManager) : base(battleManager) {
        _playerTurnStateMachine = new PlayerTurnStateMachine(this, targetManager);
    }
    
    public override void Enter(ABattleState previousState) {
        _playerTurnStateMachine.SetState(_playerTurnStateMachine.GetState(typeof(DefaultSelectionState)));
    }

    public override void Update(float deltaTime) {
        _playerTurnStateMachine.Update(deltaTime);
    }

    public override void FixedUpdate(float fixedDeltaTime) {
        _playerTurnStateMachine.FixedUpdate(fixedDeltaTime);
    }
    
    public override void Exit(ABattleState nextState) {
        
    }
}
