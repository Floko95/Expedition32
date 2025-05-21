using UnityEngine;

public class DefaultSelectionState : APlayerTurnSubState
{
    protected AllyUnit _allyUnit;

    public DefaultSelectionState(PlayerTurnState playerTurnState) : base(playerTurnState) { }

    public override void Enter(APlayerTurnSubState previousState) {
        _allyUnit = _context.CurrentTurnUnit;
        
        _allyUnit.animator.SetBool("MyTurn", true);
        _allyUnit.uiPrompt.enabled = true;
    }

    public override void Update(float deltaTime) {
        
    }

    public override void FixedUpdate(float fixedDeltaTime) {
        
    }

    public override void Exit(APlayerTurnSubState nextState) {
        _allyUnit.uiPrompt.enabled = true;
    }
}
