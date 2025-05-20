using UnityEngine;

public abstract class ACharacterState : IState<ACharacterState> {

    public Character Character;
    public CharacterStateController StateController;
    public Animator Animator;

    protected const float _crossFadeDuration = 0.1f;
    
    public ACharacterState(Character character, CharacterStateController stateController) {
        Character = character;
        StateController = stateController;
        Animator = character.GetAnimator();
    }
    
    public abstract void Enter(ACharacterState previousState);

    public abstract void Update(float deltaTime);

    public abstract void FixedUpdate(float fixedDeltaTime);

    public abstract void Exit(ACharacterState nextState);
}
