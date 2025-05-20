using Unity.Cinemachine;
using UnityEngine;

public class IdleState : ACharacterState {
    
    public IdleState(Character character, CharacterStateController stateController) : base(character, stateController) {
        
    }

    public override void Enter(ACharacterState previousState) {
        Character.rotationMode = ECM2.Character.RotationMode.OrientRotationToMovement;
    }

    public override void Update(float deltaTime) {
        
    }

    public override void FixedUpdate(float fixedDeltaTime) {
        
    }

    public override void Exit(ACharacterState nextState) {
        
    }
}
