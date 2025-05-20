using TMPro;
using UnityEngine;

public class StateMachineDebugUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpDebug;

    private StateMachine<ACharacterState> _stateMachine;

    private void Start() {
        var character = FindAnyObjectByType(typeof(CharacterStateController)) as CharacterStateController;
        _stateMachine = character?.StateMachine;
        
        if (_stateMachine != null) {
            _stateMachine.onStateChanged += UpdateUI;
        }
        else {
            Debug.LogError("StateMachineDebugUI: No state machine");
        }
    }

    private void OnDisable() {
        if(_stateMachine != null)
            _stateMachine.onStateChanged -= UpdateUI;
    }

    private void UpdateUI(ACharacterState newState) {
        tmpDebug.text = newState.GetType().ToString();
    }
}
