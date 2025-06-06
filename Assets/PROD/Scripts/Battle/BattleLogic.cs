using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class BattleLogic {
    
    public const float CRIT_DMG_MULTIPLIER = 1.5f;

    public static event Action<Unit, Unit, float, bool> DamagingEvent; //source, target, amount, IsCrit
    public static event Action<Unit, Unit, float> HealingEvent; //source, target, amount
    
    public static float HealPercent(Unit caster, Unit receiver, float maxHealthRatio) {
        if (!caster.IsAlive || !receiver.IsAlive) return 0f;

        var amount = receiver.HealthSystem.GetHealthMax() * maxHealthRatio;
        
        
        receiver.HealthSystem.Heal(amount);
        HealingEvent.Invoke(caster, receiver, amount);
        
        return amount;
    }
    
    public static float Revive(Unit caster, Unit receiver, float maxHealthRatio) {
        if (!caster.IsAlive || receiver.IsAlive) return 0f;
        
        var amount = receiver.HealthSystem.GetHealthMax() * maxHealthRatio;
        receiver.HealthSystem.Revive(amount);
        
        return amount;
    }

    public static bool TryApplyAbilityEffects(AbilityData abilityData, Unit caster, AllyUnit target) {
        if (!caster.IsAlive || !target.IsAlive) return false;
        
        var dodgeSystem = target.DodgeSystem;
        if (dodgeSystem.Evaluate(abilityData.dodgeMode)) return false; //dodged
        
        abilityData.ApplyEffects(caster, target);
        return true;
    }
    
    public static float Attack(Unit attacker, Unit defender, float attackRatio) {
        if (!attacker.IsAlive || !defender.IsAlive) return 0;
        
        bool isCrit = Random.Range(0, 100) <= attacker.CRIT;
        
        var dmgAmount = attacker.ATK * attackRatio - defender.DEF;
        if(isCrit)
            dmgAmount *= CRIT_DMG_MULTIPLIER;
        
        dmgAmount = Mathf.Max(1, dmgAmount);
        defender.HealthSystem.Damage(dmgAmount);
        
        DamagingEvent.Invoke(attacker, defender, dmgAmount, isCrit);
        return dmgAmount;
    }
}
