using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayerChoosesTargetAction", story: "Played [Unit] chose its next [targets]", category: "Action", id: "dd209b773023c23efc9e4a22f5d96e7c")]
public partial class PlayerChoosesTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<AbilityData> Ability;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Targets;
    
    private TargetManager _targetManager;
    
    protected override Status OnStart() {
        _targetManager ??= Toolbox.Get<TargetManager>();
        
        Targets.Value = _targetManager.CurrentlyTargeted.Select(t => t.gameObject).ToList();
        _targetManager.Reset();
        
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

