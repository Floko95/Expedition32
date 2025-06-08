using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ResetTargetModeAction", story: "Reset [TargetManager] mode", category: "Action", id: "84968c78d07b42912303ff08852c7d40")]
public partial class ResetTargetModeAction : Action
{
    [SerializeReference] public BlackboardVariable<TargetManager> TargetManager;

    protected override Status OnStart()
    {
        TargetManager.Value.Reset();
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

