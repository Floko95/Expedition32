using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SkipTurnAction", story: "[Unit] Skips its turn", category: "Action", id: "3c38aa633add56259fdce39efc08b41d")]
public partial class SkipTurnAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Unit;

    protected override Status OnStart() {
        BattleLogDebugUI.Log($"{Unit.Value.unitData.unitName} skips its turn... ");
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

