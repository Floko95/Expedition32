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

    private void Start() {
        HealthSystem.OnDamaged += OnDamaged;
        HealthSystem.OnDead += OnDeath;
    }

    private void OnDestroy() {
        HealthSystem.OnDamaged -= OnDamaged;
        HealthSystem.OnDead -= OnDeath;
    }

    private void OnDamaged() {
        animator.SetTrigger("Damaged");
    }
    
    private void OnDeath() {
        animator.SetBool("IsDead", true);
        
    }
}
