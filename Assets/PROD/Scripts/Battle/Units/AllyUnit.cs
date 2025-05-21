using System.Collections;
using UnityEngine;

public class AllyUnit : Unit {

    [SerializeField] public Canvas uiPrompt;
    [SerializeField] public Animator animator;
    
    private TargetManager _targetManager;
    
    protected override void Awake() {
        base.Awake();
        uiPrompt.enabled = false;
    }

    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _targetManager = Toolbox.Get<TargetManager>();
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
