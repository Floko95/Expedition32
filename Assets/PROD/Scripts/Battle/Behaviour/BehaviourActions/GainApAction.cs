using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GainAPAction", story: "[Unit] Gains [x] AP", category: "Action", id: "78dcb691a44eda5b8d6c3967d7157f77")]
public partial class GainApAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<int> X;

    protected override Status OnStart()
    {
        Unit.Value.APSystem.GiveAP(X.Value);
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

