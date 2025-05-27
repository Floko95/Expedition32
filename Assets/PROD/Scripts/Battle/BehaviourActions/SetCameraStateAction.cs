using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetCameraStateAction", story: "Sets [EnumDrivenCamera] 's State to [CameraState]", category: "Action", id: "de8f44b254855f931999eeb5f878fbe5")]
public partial class SetCameraStateAction : Action
{
    [SerializeReference] public BlackboardVariable<EnumDrivenCamera> EnumDrivenCamera;
    [SerializeReference] public BlackboardVariable<CameraState> CameraState;
    
    protected override Status OnStart() {
        if (EnumDrivenCamera.Value == null)
            return Status.Failure;

        EnumDrivenCamera.Value.CurrentState = CameraState.Value;
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

