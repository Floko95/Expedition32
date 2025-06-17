using UnityEngine;

public class AllyUnit : Unit {

    //IDK where to put those
    [SerializeField] public Canvas uiPrompt;
    [SerializeField] public Canvas uiAbilities;
    [SerializeField] public Canvas uiItems;

    [SerializeField] private ShaderBloodController shaderBloodController;
    [SerializeField] private DodgeSystem _dodgeSystem;
    
    [SerializeField] private AUniqueMechanicSystem _uniqueMechanicSystem;
    
    [SerializeReference] private AbilityEffect onParryEffect;
    
    public DodgeSystem DodgeSystem => _dodgeSystem;
    public AUniqueMechanicSystem UniqueMechanicSystem => _uniqueMechanicSystem;
    
    private AUniqueMechanicSystem _aUniqueMechanic;
    
    protected override void Awake() {
        base.Awake();
        uiPrompt.enabled = false;
        uiAbilities.enabled = false;
        uiItems.enabled = false;
        
        _dodgeSystem.enabled = false;
    }

    protected override void Start() {
        base.Start();
        HealthSystem.OnDamaged += OnDamaged;
        HealthSystem.OnDead += OnDeath;
        HealthSystem.OnRevived += OnRevived;
        DodgeSystem.OnParry += OnParry;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        HealthSystem.OnDamaged -= OnDamaged;
        HealthSystem.OnDead -= OnDeath;
        HealthSystem.OnRevived -= OnRevived;
        DodgeSystem.OnParry -= OnParry;
    }

    private void OnDamaged() {
        shaderBloodController.BloodAmountNormalized = 1 - HealthSystem.GetHealthNormalized();
    }
    
    private void OnParry() {
        onParryEffect.Apply(this, this);
    }
    
    private void OnDeath() {
        DodgeSystem.enabled = false;
    }
    
    private void OnRevived() {
        animator.SetTrigger("Revive");
        animator.SetBool("IsDead", false);
    }
}
