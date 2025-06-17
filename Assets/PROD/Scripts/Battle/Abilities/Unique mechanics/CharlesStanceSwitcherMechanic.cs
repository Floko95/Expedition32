using UnityEngine;

public class CharlesStanceSwitcherMechanic : AUniqueMechanicSystem {
    
    [SerializeField] private Stance DeriveStance;
    [SerializeField] private Stance EclipseStance;
    [SerializeField] private int ADRatioForThresholds;
    
    public Stance CurrentCycle => _currentCycle;
    public override string Title => CurrentCycle.name;
    public override string Description => CurrentCycle.description;
    public override Sprite Icon => CurrentCycle.icon;
    
    
    private Stance _currentCycle;
    private float _DamageAndHealAmountThreshold;
    
    public override void Init(Unit data) {
        base.Init(data);
        
        if(_unit == null) return;
        SetStance(DeriveStance);
    }

    public void SetStance(Stance cycle) {
        if(cycle == _currentCycle) return;

        if (_currentCycle != null) {
            foreach (var effect in _currentCycle.effects) {
                effect.Cancel(_unit, _unit);
            }
        }
        
        _currentCycle = cycle;
        _DamageAndHealAmountThreshold = _unit.ATK * ADRatioForThresholds;
        
        foreach (var effect in _currentCycle.effects) {
            effect.Apply(_unit, _unit);
        }
    }

    protected override void OnUnitHealed(Unit source, Unit target, float amount) {
        base.OnUnitHealed();
        if(source != _unit) return;
        if (_currentCycle != DeriveStance) return;
        
        _DamageAndHealAmountThreshold -= amount;
        
        if (_DamageAndHealAmountThreshold <= 0) {
            SetStance(EclipseStance);
        }
    }

    protected override void OnDamageInflictedPostMitigation(Unit source, Unit target, float amount, bool isCrit, ElementType damageType, ElementReaction elementalReaction) {
        base.OnDamageInflictedPostMitigation(source, target, amount, isCrit, damageType, elementalReaction);
        
        if(source != _unit) return;
        if (_currentCycle != EclipseStance) return;
        
        _DamageAndHealAmountThreshold -= amount;

        if (_DamageAndHealAmountThreshold <= 0) {
            SetStance(DeriveStance);
        }
    }
}

