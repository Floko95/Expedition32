using System;
using Unity.Cinemachine;
using UnityEngine;

public class CharacterStateController : MonoBehaviour
{
    [SerializeField] public CinemachineVirtualCameraBase freeAimVCam;
    
    public Character character { get; set; }
    public CharacterInput characterInput { get; set; }
    public CharacterStateMachine StateMachine => _stateMachine;
    
    private CharacterStateMachine _stateMachine;
    
    private void Awake() {
        character = GetComponent<Character>();
        characterInput = GetComponent<CharacterInput>();
    }

    private void Start() {
        _stateMachine = new CharacterStateMachine(characterInput, character, this);
    }

    private void Update() {
        _stateMachine.Update(Time.deltaTime);
    }

    private void FixedUpdate() {
        _stateMachine.FixedUpdate(Time.fixedDeltaTime);
    }
}
