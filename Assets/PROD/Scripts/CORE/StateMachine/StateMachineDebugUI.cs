using TMPro;
using UnityEngine;

public class StateMachineDebugUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpDebug;
    [SerializeField] private DodgeSystem dodgeSystem;
    
    private DodgeStateMachine _dodgeStateMachine;
    
    private void Start() {
        _dodgeStateMachine = dodgeSystem?.StateMachine;
        
        if (_dodgeStateMachine != null) {
            _dodgeStateMachine.onStateChanged += UpdateUI;
        }
        else {
            Debug.LogError("StateMachineDebugUI: No state machine");
        }
    }

    private void UpdateUI(DodgeState newState) {
        tmpDebug.text = newState.GetType().ToString();
    }

    private void OnDisable() {
        if(_dodgeStateMachine != null)
            _dodgeStateMachine.onStateChanged -= UpdateUI;
    }
}
