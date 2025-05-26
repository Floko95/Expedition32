using UnityEngine;

public static class BattleLogic
{
    public static float Attack(Unit attacker, Unit defender) { //TODO add ratio
        if (defender.IsAlive == false) return 0;
        
        var dmgAmount = attacker.ATK - defender.DEF;
        
        defender.HealthSystem.Damage(dmgAmount);
        
        return dmgAmount;
    }
}
