using System;
using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CameraFollowAction", story: "[VCam] follows [Target]", category: "Action", id: "f1252597899e5186c92fa18cd2580f30")]
public partial class CameraFollowAction : Action
{
    [SerializeReference] public BlackboardVariable<CinemachineVirtualCameraBase> VCam;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    protected override Status OnStart()
    {
        VCam.Value.Follow = Target.Value;
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

