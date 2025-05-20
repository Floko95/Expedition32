using UnityEngine;

public sealed class CharacterStateMachine : StateMachine<ACharacterState>
{
    private IdleState _idleState;
    private FreeAimState _freeAimState;
    private SprintingState _sprintingState;
    private JumpState _jumpState;
    
    public CharacterStateMachine(CharacterInput input, Character character, CharacterStateController stateController) {
        if(character == null)
            Debug.LogError("Character object is null");
        
        IdleState idleState = new IdleState(character, stateController);
        FreeAimState freeAimState = new FreeAimState(character, stateController);
        SprintingState sprintingState = new SprintingState(character, stateController);
        JumpState jumpState = new JumpState(character, stateController);
        
        AddState(idleState);
        AddState(freeAimState);
        AddState(sprintingState);
        AddState(jumpState);
        
        AddTransition(idleState, freeAimState, () => input.IsAiming);
        AddTransition(freeAimState, idleState, () => !input.IsAiming);
        AddTransition(idleState, sprintingState, () => input.IsSprinting);
        AddTransition(sprintingState, idleState, () => !input.IsSprinting);
        
        AddTransition(idleState, jumpState, () => input.IsJumping);
        AddTransition(jumpState, idleState, character.IsGrounded);
        AddTransition(sprintingState, jumpState, () => input.IsJumping);
        
        SetState(idleState);
    }
}
