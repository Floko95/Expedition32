using System;
using Unity.Behavior;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AbilityMenuAction", story: "Set [Unit] 's Ability menu to [Active]", category: "Action/UI", id: "6705e15dc1201917c3cbe941698b2ec9")]
public partial class AbilityMenuAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<bool> Active;
    
    private AllyUnit _allyUnit;
    
    protected override Status OnStart() {
        
        _allyUnit = Unit.Value as AllyUnit;
        _allyUnit.WorldUI.enabled = true;
        
        _allyUnit.uiAbilities.enabled = Active.Value;
        
        return Status.Running;
    }
    
    protected override Status OnUpdate() {
        return Status.Success;
    }

    protected override void OnEnd() {
    }
}

