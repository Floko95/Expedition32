using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetAbilityFromUnitAction", story: "Set [Ability] to [Unit] 's ability number [int]", category: "Action", id: "15080a217aff3835f93da167c2d5c9fe")]
public partial class GetAbilityFromUnitAction : Action
{
    [SerializeReference] public BlackboardVariable<AbilityData> Ability;
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<int> Int;

    protected override Status OnStart()
    {
        if (Unit.Value.Abilities.Count <= Int.Value 
            || Unit.Value.Abilities[Int.Value] == null
            || Unit.Value.APSystem.CanSpendAP(Unit.Value.Abilities[Int.Value].costAP) == false) {
            return Status.Failure;
        }
        
        Ability.Value = Unit.Value.Abilities[Int.Value];
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

