using System;
using UnityEngine;

public class AllyUnit : Unit {

    [SerializeField] public Canvas uiPrompt;
    [SerializeField] public Canvas uiAbilities;
    
    [SerializeField] public Animator animator;
    
    protected override void Awake() {
        base.Awake();
        uiPrompt.enabled = false;
        uiAbilities.enabled = false;
    }
    
}
