using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BraceAction", story: "[TargetedUnits] brace themselves", category: "Action", id: "d7b37977c0cf9894e029c772121dbb69")]
public partial class BraceAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> AttackingUnit;
    [SerializeReference] public BlackboardVariable<List<GameObject>> TargetedUnits;
    [SerializeReference] public BlackboardVariable<bool> enable;
    
    private List<AllyUnit> _targetedUnits = new List<AllyUnit>();
    
    protected override Status OnStart()
    {
        _targetedUnits = TargetedUnits.Value.Select(g => g.GetComponent<AllyUnit>()).Where(u => u != null).ToList();
        
        foreach (var target in _targetedUnits) {
            target.DodgeSystem.enabled = enable.Value;
        }
        
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

