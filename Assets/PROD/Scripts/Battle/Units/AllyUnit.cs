using UnityEngine;

public class AllyUnit : Unit {

    [SerializeField] public Canvas uiPrompt;
    [SerializeField] public Animator animator;
    
    protected override void Awake() {
        base.Awake();
        uiPrompt.enabled = false;
    }
}
