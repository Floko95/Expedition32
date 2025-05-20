using UnityEngine;

public class FreeAimState : ACharacterState
{
    private static readonly int Strafe = Animator.StringToHash("Strafing");
    public FreeAimState(Character character, CharacterStateController stateController) : base(character, stateController) {
        
    }

    public override void Enter(ACharacterState previousState) {
        StateController.freeAimVCam.Priority.Value = 11;
        Animator.SetBool(Strafe, true);
        Character.rotationMode = ECM2.Character.RotationMode.OrientRotationToViewDirection;
    }

    public override void Update(float deltaTime) {
       
    }
    
    public override void FixedUpdate(float fixedDeltaTime) {
        
    }

    public override void Exit(ACharacterState nextState) {
        StateController.freeAimVCam.Priority.Value = 9;
        Animator.SetBool(Strafe, false);
    }
}
