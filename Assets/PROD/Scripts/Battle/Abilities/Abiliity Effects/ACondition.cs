using System;
using UnityEngine;

abstract class ACondition {
    public abstract bool IsConditionMet(Unit caster, Unit Target);
}

[Serializable]
class DoesTargetHaveStatusCondition : ACondition {

    [SerializeField] private StatusData status;
    
    public override bool IsConditionMet(Unit caster, Unit Target) {
        return Target.StatusSystem.HasStatus(status);
    }
}

[Serializable]
class IsCasterInStance : ACondition {

    [SerializeField] private Stance stance;
    
    public override bool IsConditionMet(Unit caster, Unit Target) {
        //caster is allegedly a Stance user
        
        var StanceMechanic = caster.GetComponent<GeoffreyStanceCyclerMechanic>();
        return StanceMechanic.CurrentStance.name == stance.name;
    }
}

