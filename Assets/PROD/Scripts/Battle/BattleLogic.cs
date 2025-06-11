using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class BattleLogic {
    
    public const float CRIT_DMG_MULTIPLIER = 1.5f;

    public static event Action<Unit, Unit, float, bool, ElementType, ElementReaction> DamagingEvent; //source, target, amount, IsCrit, damageType, elementalReaction
    public static event Action<Unit, Unit, float> HealingEvent; //source, target, amount
    
    public static float HealPercent(Unit caster, Unit receiver, float maxHealthRatio) {
        if (!caster.IsAlive || !receiver.IsAlive) return 0f;

        var amount = receiver.HealthSystem.GetHealthMax() * maxHealthRatio;
        
        
        receiver.HealthSystem.Heal(amount);
        HealingEvent?.Invoke(caster, receiver, amount);
        
        return amount;
    }
    
    public static float Revive(Unit caster, Unit receiver, float maxHealthRatio) {
        if (!caster.IsAlive || receiver.IsAlive) return 0f;
        
        var amount = receiver.HealthSystem.GetHealthMax() * maxHealthRatio;
        receiver.HealthSystem.Revive(amount);
        
        return amount;
    }

    public static (bool negated, bool parried) TryApplyAbilityEffects(AbilityData abilityData, Unit caster, AllyUnit target) {
        if (!caster.IsAlive || !target.IsAlive) return (false, false);
        
        var dodgeEvaluation = target.DodgeSystem.Evaluate(abilityData.dodgeMode);
        if (dodgeEvaluation.negated == false)
            abilityData.ApplyEffects(caster, target);
        
        return dodgeEvaluation;
    }
    
    public static float Attack(Unit attacker, Unit defender, ElementType damageType, float attackRatio) {
        if (!attacker.IsAlive || !defender.IsAlive) return 0;
        
        bool isCrit = Random.Range(0, 100) <= attacker.CRIT;
        var dmgAmount = attacker.ATK * attackRatio - defender.DEF;
        dmgAmount *= isCrit ? CRIT_DMG_MULTIPLIER : 1;
        dmgAmount = Mathf.Max(1, dmgAmount);
        
        
        var reactionCouple = GetElementalReaction(defender, damageType);
        //Debug.Log($"{defender.unitData.unitName} is {reactionCouple.reaction} to {damageType}");
        
        if (reactionCouple.reaction is ElementReaction.Absorb) {
            defender.HealthSystem.Heal(dmgAmount);
            return dmgAmount;
        } 
        dmgAmount *= reactionCouple.multiplier;
        
        defender.HealthSystem.Damage(dmgAmount);

        DamagingEvent?.Invoke(attacker, defender, dmgAmount, isCrit, damageType, reactionCouple.reaction);
        return dmgAmount;
    }
    
    public static (ElementReaction reaction, float multiplier) GetElementalReaction(Unit target, ElementType type) {
        return (target.unitData.GetReaction(type), target.unitData.GetReaction(type) switch {
            ElementReaction.Weak => 2f,
            ElementReaction.Resistant => 0.5f,
            ElementReaction.Immune => 0f,
            ElementReaction.Absorb => -1f,
            _ => 1f
        });
    }
}
