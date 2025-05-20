using System.Collections;
using UnityEngine;

public class AllyUnit : Unit {

    [SerializeField] private Canvas uiPrompt;
    [SerializeField] public Animator animator;
    
    public AllyStateMachine _allyTurnStateMachine;
    
    protected override void Awake() {
        base.Awake();
        
        _allyTurnStateMachine = new AllyStateMachine();
    }

    private void Start() {
        uiPrompt.enabled = false;
    }
    
    public override IEnumerator ExecuteTurn() {
        yield return PromptTurn();
    }

    protected virtual IEnumerator PromptTurn() {
        uiPrompt.enabled = true;
        
        while (true) {
            yield return null;
        }
    }
}
