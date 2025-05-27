using System;
using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CameraPriorityAction", story: "Set [VCam] Priority to [int]", category: "Action", id: "0c1512221704071b741dc31a01d9e856")]
public partial class CameraPriorityAction : Action
{
    [SerializeReference] public BlackboardVariable<CinemachineVirtualCameraBase> VCam;
    [SerializeReference] public BlackboardVariable<int> Int;

    protected override Status OnStart() {
        if (VCam.Value)
            VCam.Value.Priority = Int.Value;
        
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

