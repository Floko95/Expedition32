using UnityEngine;

public class SprintingState : ACharacterState {
    
    private static readonly int Sprinting = Animator.StringToHash("Sprinting");
    
    private float _initialWalkingSpeed;
    
    public SprintingState(Character character, CharacterStateController stateController) : base(character, stateController) {
        _initialWalkingSpeed = character.maxWalkSpeed;
    }

    public override void Enter(ACharacterState previousState) {
        Character.maxWalkSpeed = _initialWalkingSpeed * 2f;
        
        Animator.SetBool(Sprinting, true);
    }

    public override void Update(float deltaTime) {
        
    }

    public override void FixedUpdate(float fixedDeltaTime) {
        
    }

    public override void Exit(ACharacterState nextState) {
        Character.maxWalkSpeed = _initialWalkingSpeed;
        
        Animator.SetBool(Sprinting, false);
    }
}
