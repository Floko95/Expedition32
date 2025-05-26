using UnityEngine;

public static class BattleLogic {
    
    public const float CRIT_DMG_MULTIPLIER = 1.5f;
    
    public static float Attack(Unit attacker, Unit defender, float attackRatio) { //TODO add ratio
        if (defender.IsAlive == false) return 0;
        
        bool isCrit = Random.Range(0, 100) <= attacker.CRIT;
        
        var dmgAmount = attacker.ATK * attackRatio - defender.DEF;
        if(isCrit)
            dmgAmount *= CRIT_DMG_MULTIPLIER;
        
        defender.HealthSystem.Damage(dmgAmount);
        
        return dmgAmount;
    }
}
