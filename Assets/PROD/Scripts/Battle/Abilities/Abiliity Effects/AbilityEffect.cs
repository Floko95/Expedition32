using System;
using OSLib.StatSystem;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public abstract class AbilityEffect {
    
    public float Efficiency {
        get => efficiency;
        set => efficiency = Mathf.Clamp01(value);
    }

    public abstract void Apply(Unit caster, Unit target);
    public virtual void Cancel(Unit caster, Unit target) { }
    protected float efficiency = 1.0f;
}

//TODO apply efficiency
[Serializable]
class DamageEffect : AbilityEffect {
    
    [SerializeField] private float attackRatio = 1f;
    [SerializeField] private ElementType damageType;
    
    public override void Apply(Unit caster, Unit target) {
        BattleLogic.Attack(caster, target, damageType, attackRatio);
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
class ReviveEffect : AbilityEffect {
    
    [SerializeField] private float maxHealthHealRatio = 0.3f;
    
    public override void Apply(Unit caster, Unit target) {
        BattleLogic.Revive(caster, target, maxHealthHealRatio);
    }
}

[Serializable]
class GainAPEffect : AbilityEffect {
    
    [SerializeField] private int apRegained = 1;
    
    public override void Apply(Unit caster, Unit target) {
        caster.APSystem.GiveAP(apRegained);
    }
}

[Serializable]
class GiveAPToEffect : AbilityEffect {
    
    [SerializeField] private int apRegained = 1;
    
    public override void Apply(Unit caster, Unit target) {
        target.APSystem.GiveAP(apRegained);
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

[Serializable]
class ApplyStatModifierEffect: AbilityEffect {

    [SerializeField] private StatType statType;
    [SerializeField] private StatModifier modifier;
    
    public override void Apply(Unit caster, Unit target) {
        if(!Mathf.Approximately(efficiency, 1f))
            modifier.value *= efficiency;
        
        target.GetStatSystem().stats[statType].AddModifier(modifier);
    }

    public override void Cancel(Unit caster, Unit target) {
        if(!Mathf.Approximately(efficiency, 1f))
            modifier.value /= efficiency;
        
        target.GetStatSystem().stats[statType].RemoveModifier(modifier);
    }
}

[Serializable]
class InstantiateAtTargetEffect: AbilityEffect {

    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector3 offset;
    
    public override void Apply(Unit caster, Unit target) {
        Object.Instantiate(prefab, target.transform.position + offset, Quaternion.identity, target.transform);
    }
}

abstract class GeoffreyEffect : AbilityEffect { }

class KeepStanceEffect : GeoffreyEffect {
    
    public override void Apply(Unit caster, Unit target) {
        var mechanic = caster.GetComponent<GeoffreyStanceCyclerMechanic>();
        mechanic.SkipNextAbilityUsed();
    }
}

class InterruptWorkoutRoutineEffect : GeoffreyEffect {
    public override void Apply(Unit caster, Unit target) {
        var mechanic = caster.GetComponent<GeoffreyStanceCyclerMechanic>();
        
        mechanic.ResetChain();
    }
}
