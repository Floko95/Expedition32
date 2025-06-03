using UnityEngine;

public static class BattleLogic {
    
    public const float CRIT_DMG_MULTIPLIER = 1.5f;

    public static float HealPercent(Unit caster, Unit receiver, float maxHealthRatio) {
        if (!caster.IsAlive || !receiver.IsAlive) return 0f;

        var amount = receiver.HealthSystem.GetHealthMax() * maxHealthRatio;
        
        receiver.HealthSystem.Heal(amount);
        return amount;
    }
    
    public static float Revive(Unit caster, Unit receiver, float maxHealthRatio) {
        if (!caster.IsAlive || receiver.IsAlive) return 0f;
        
        var amount = receiver.HealthSystem.GetHealthMax() * maxHealthRatio;
        receiver.HealthSystem.Revive(amount);
        
        return amount;
    }

    public static void EnemyTriesToApplyEffects(Unit caster, AllyUnit receiver) {
        
    }
    
    public static float Attack(Unit attacker, Unit defender, float attackRatio) {
        if (!attacker.IsAlive || !defender.IsAlive) return 0;
        
        bool isCrit = Random.Range(0, 100) <= attacker.CRIT;
        
        var dmgAmount = attacker.ATK * attackRatio - defender.DEF;
        if(isCrit)
            dmgAmount *= CRIT_DMG_MULTIPLIER;
        
        defender.HealthSystem.Damage(dmgAmount);
        
        return dmgAmount;
    }
}
