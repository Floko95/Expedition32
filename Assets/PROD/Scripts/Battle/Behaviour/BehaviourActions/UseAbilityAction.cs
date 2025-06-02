using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UseItemAction", story: "Set [UsedAbility] to [Ability]", category: "Action", id: "ffb9754a5155a83d614ccaff8b117b61")]
public partial class UseAbilityAction : Action
{
    [SerializeReference] public BlackboardVariable<AbilityData> Ability;
    [SerializeReference] public BlackboardVariable<AbilityData> UsedAbility;
    
    protected override Status OnStart()
    {
        UsedAbility.Value = Ability.Value;
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

