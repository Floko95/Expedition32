using UnityEngine;

public class CharacterAnimator : MonoBehaviour {

    // Cache Animator parameters
    private static readonly int Forward = Animator.StringToHash("Forward");
    private static readonly int Turn = Animator.StringToHash("Turn");
    private static readonly int Ground = Animator.StringToHash("Grounded");
    private static readonly int Crouch = Animator.StringToHash("Crouch");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int JumpLeg = Animator.StringToHash("JumpLeg");

    // Cached Character

    public Animator Animator => _character?.GetAnimator();
    
    private Character _character;

    private void Awake() {
        // Cache our Character
        _character = GetComponentInParent<Character>();
    }

    private void Update() {
        float deltaTime = Time.deltaTime;

        // Get Character animator

        Animator animator = _character.GetAnimator();

        // Compute input move vector in local space
        Vector3 move = transform.InverseTransformDirection(_character.GetMovementDirection());

        // Update the animator parameters
        float forwardAmount = _character.useRootMotion && _character.GetRootMotionController() ? move.z : Mathf.InverseLerp(0.0f, _character.GetMaxSpeed(), _character.GetSpeed());
        float turnAmount = Mathf.Atan2(move.x, move.z);
        
        if (_character.rotationMode is ECM2.Character.RotationMode.OrientRotationToViewDirection) {
            forwardAmount = Vector3.Dot(transform.InverseTransformDirection(_character.transform.forward), move);
        }
        
        animator.SetFloat(Forward, forwardAmount, 0.1f, deltaTime);
        animator.SetFloat(Turn, turnAmount, 0.1f, deltaTime);
        
        animator.SetBool(Ground, _character.IsGrounded());
        animator.SetBool(Crouch, _character.IsCrouched());

        if (_character.IsFalling())
            animator.SetFloat(Jump, _character.GetVelocity().y, 0.1f, deltaTime);

        // Calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)

        float runCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.2f, 1.0f);
        float jumpLeg = (runCycle < 0.5f ? 1.0f : -1.0f) * forwardAmount;

        if (_character.IsGrounded())
            animator.SetFloat(JumpLeg, jumpLeg);
    }

}
