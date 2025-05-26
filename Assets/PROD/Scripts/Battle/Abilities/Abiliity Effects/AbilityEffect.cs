using System;
using UnityEngine;

[Serializable]
public abstract class AbilityEffect {
    public abstract void Apply(Unit caster, Unit target);
}

[Serializable]
class DamageEffect : AbilityEffect {
    
    [SerializeField] private int attackRatio;
    
    public override void Apply(Unit caster, Unit target) {
        BattleLogic.Attack(caster, target, attackRatio);
    }
}
