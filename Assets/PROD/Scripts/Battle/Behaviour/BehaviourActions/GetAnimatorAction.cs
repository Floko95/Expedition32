using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetAnimatorAction", story: "Get [Unit] 's [Animator]", category: "Action", id: "b0df1bdfa3969b59fc8db3f94ce80645")]
public partial class GetAnimatorAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<Animator> Animator;

    protected override Status OnStart() {
        Animator.Value = Unit.Value.animator;
        
        return Status.Running;
    }

    protected override Status OnUpdate() {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

