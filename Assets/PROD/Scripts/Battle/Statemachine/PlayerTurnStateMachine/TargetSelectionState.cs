using UnityEngine;

public class TargetSelectionState : APlayerTurnSubState
{
    protected TargetManager _targetManager;
    
    public TargetSelectionState(PlayerTurnState playerTurnState, TargetManager targetManager) : base(playerTurnState) {
        _targetManager = targetManager;
    }

    public override void Enter(APlayerTurnSubState previousState) {
        
    }

    public override void Update(float deltaTime) {
        
    }

    public override void FixedUpdate(float fixedDeltaTime) {
        
    }

    public override void Exit(APlayerTurnSubState nextState) {
        
    }
}
