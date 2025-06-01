using System;
using UnityEngine;

[Serializable]
public abstract class AbilityEffect {
    public abstract void Apply(Unit caster, Unit target);
}

[Serializable]
class DamageEffect : AbilityEffect {
    
    [SerializeField] private float attackRatio = 1f;
    
    public override void Apply(Unit caster, Unit target) {
        BattleLogic.Attack(caster, target, attackRatio);
    }
}

[Serializable]
class HealEffect : AbilityEffect {
    
    [SerializeField] private float maxHealthHealRatio = 1f;
    
    public override void Apply(Unit caster, Unit target) {
        BattleLogic.HealPercent(caster, target, maxHealthHealRatio);
    }
}

[Serializable]
class ApplyStatusEffect : AbilityEffect {
    
    [SerializeField] private StatusData appliedStatus;
    [SerializeField] private int stacksApplied;
    
    public override void Apply(Unit caster, Unit target) {
        target.StatusSystem.ApplyStatus(appliedStatus, target, stacksApplied);
    }
}
