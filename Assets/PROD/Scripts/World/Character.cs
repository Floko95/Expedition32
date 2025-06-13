using UnityEngine;

public class Character : ECM2.Character {
    [SerializeField] private float sprintMaxSpeed;
    [SerializeField] private float sprintAcceleration;
    [SerializeField] private float sprintDeceleration;

    public CharacterStateController StateController { get; set; }
    public CharacterInput CharacterInput { get; set; }

    protected override void Awake() {
        base.Awake();
        StateController = GetComponent<CharacterStateController>();
        CharacterInput = GetComponent<CharacterInput>();
    }

    protected override void Start() {
        base.Start();
        camera = Camera.main;
    }
}



