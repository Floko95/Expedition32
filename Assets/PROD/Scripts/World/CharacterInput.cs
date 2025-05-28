using ECM2;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInput : MonoBehaviour {
    
    [SerializeField, Space(15f)] private PlayerInput _playerInput;
    [SerializeField] private Character _character;
    
    public Character character => _character;
    
    public bool IsJumping => JumpInputAction.inProgress;
    public bool IsSprinting => SprintInputAction.inProgress;
    public bool IsAiming => FreeAimInputAction.inProgress;
    
    public InputActionAsset InputActionsAsset {
        get => _playerInput?.actions;
        set {
            DeinitPlayerInput();

            _playerInput.actions = value;
            InitPlayerInput();
        }
    }

    public InputAction MovementInputAction { get; set; }

    public InputAction JumpInputAction { get; set; }
    
    public InputAction SprintInputAction { get; set; }
    public InputAction FreeAimInputAction { get; set; }

    protected virtual void Awake() {
        if (_character == null) {
            _character = GetComponent<Character>();
        }
    }

    protected virtual void OnEnable() {
        InitPlayerInput();
    }

    protected virtual void OnDisable() {
        DeinitPlayerInput();
    }

    protected virtual void Update() {
        HandleInput();
    }
    
    protected virtual void InitPlayerInput() {
        if (InputActionsAsset == null)
            return;

        MovementInputAction = InputActionsAsset.FindAction("Move");
        MovementInputAction?.Enable();

        JumpInputAction = InputActionsAsset.FindAction("Jump");
        JumpInputAction?.Enable();
    
        SprintInputAction = InputActionsAsset.FindAction("Sprint");
        SprintInputAction?.Enable();

        FreeAimInputAction = InputActionsAsset.FindAction("Aim");
        FreeAimInputAction?.Enable();
    }

    protected virtual void DeinitPlayerInput() {
        // Unsubscribe from input action events and disable input actions

        if (MovementInputAction != null) {
            MovementInputAction.Disable();
            MovementInputAction = null;
        }

        if (JumpInputAction != null) {
            JumpInputAction.Disable();
            JumpInputAction = null;
        }

        if (SprintInputAction != null) {
            SprintInputAction.Disable();
            SprintInputAction = null;
        }

        if (FreeAimInputAction != null) {
            FreeAimInputAction.Disable();
            FreeAimInputAction = null;
        }
    }

    public virtual Vector2 GetMovementInput() {
        return MovementInputAction?.ReadValue<Vector2>() ?? Vector2.zero;
    }

    protected virtual void HandleInput() {
        
        // Should this character handle input ?
        if (InputActionsAsset == null)
            return;

        // Poll movement InputAction

        Vector2 movementInput = GetMovementInput();

        Vector3 movementDirection = Vector3.zero;

        movementDirection += Vector3.right * movementInput.x;
        movementDirection += Vector3.forward * movementInput.y;

        // If character has a camera assigned...

        if (_character.camera) {
            // Make movement direction relative to its camera view direction

            movementDirection = movementDirection.relativeTo(_character.cameraTransform, _character.GetUpVector());
        }

        // Set character's movement direction vector
        _character.SetMovementDirection(movementDirection);
    }
}