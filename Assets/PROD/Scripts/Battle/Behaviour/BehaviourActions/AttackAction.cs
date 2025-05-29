using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackAction", story: "[Unit] will use it's Attack [Ability]", category: "Action", id: "40c642591e27dd083b8974d6762a57c4")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<AbilityData> Ability;

    protected override Status OnStart() {
        Ability.Value = Unit.Value.unitData.attackAbility;
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

