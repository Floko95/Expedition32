using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChooseTarget", story: "Chooses the next [Target]", category: "Action", id: "761f0174f7b60094ece77b04c7156290")]
public partial class ChooseTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Target;

    protected override Status OnStart()
    {
        //for test purposes
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

