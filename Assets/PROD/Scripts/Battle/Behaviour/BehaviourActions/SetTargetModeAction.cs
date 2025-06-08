using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetTargetModeAction", story: "Set [TargetManager] Mode to [Ability] 's TargetMode", category: "Action", id: "5e2bda19e4c569a997f056dc16196539")]
public partial class SetTargetModeAction : Action
{
    [SerializeReference] public BlackboardVariable<TargetManager> TargetManager;
    [SerializeReference] public BlackboardVariable<AbilityData> Ability;
    
    protected override Status OnStart() {
        
        TargetManager.Value.TargetMode = Ability.Value.targetMode;
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

