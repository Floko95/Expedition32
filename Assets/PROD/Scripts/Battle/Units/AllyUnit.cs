using UnityEngine;

public class AllyUnit : Unit {

    //IDK where to put those
    [SerializeField] public Canvas uiPrompt;
    [SerializeField] public Canvas uiAbilities;
    [SerializeField] public Canvas uiItems;

    [SerializeField] private ShaderBloodController shaderBloodController;
    [SerializeField] private DodgeSystem _dodgeSystem;
    
    public DodgeSystem DodgeSystem => _dodgeSystem;
    
    protected override void Awake() {
        base.Awake();
        uiPrompt.enabled = false;
        uiAbilities.enabled = false;
        uiItems.enabled = false;
        
        _dodgeSystem.enabled = false;
    }

    private void Start() {
        HealthSystem.OnDamaged += OnDamaged;
        HealthSystem.OnDead += OnDeath;
        HealthSystem.OnRevived += OnRevived;
    }

    

    private void OnDestroy() {
        HealthSystem.OnDamaged -= OnDamaged;
        HealthSystem.OnDead -= OnDeath;
        HealthSystem.OnRevived -= OnRevived;
    }

    private void OnDamaged() {
        shaderBloodController.BloodAmountNormalized = 1 - HealthSystem.GetHealthNormalized();
        animator.SetTrigger("Damaged");
    }
    
    private void OnDeath() {
        animator.SetBool("IsDead", true);
    }
    
    private void OnRevived() {
        animator.SetTrigger("Revive");
        animator.SetBool("IsDead", false);
    }
}
