using UnityEngine;

public class JumpState : ACharacterState
{
    private static readonly int Jump = Animator.StringToHash("Jump");
    
    public JumpState(Character character, CharacterStateController stateController) : base(character, stateController) {
    }

    public override void Enter(ACharacterState previousState) {
        Animator.CrossFade(Jump, _crossFadeDuration);
        Character.Jump();
    }

    public override void Update(float deltaTime) {
        
    }

    public override void FixedUpdate(float fixedDeltaTime) {
        
    }

    public override void Exit(ACharacterState nextState) {
        Character.StopJumping();
    }
}
