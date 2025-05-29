using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ItemMenuAction", story: "Set [Unit] 's Item menu to [Active]", category: "Action",
    id: "f0f9834fbc9e01ee71b81a1fd1645e84")]
public partial class ItemMenuAction : Action {
    
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<bool> Active;

    private AllyUnit _allyUnit;
    
    protected override Status OnStart() {
        _allyUnit = Unit.Value as AllyUnit;
        _allyUnit.WorldUI.enabled = true;
        
        _allyUnit.uiItems.enabled = Active.Value;
        
        return Status.Running;
    }

    protected override Status OnUpdate() {
        return Status.Success;
    }

    protected override void OnEnd() {
    }
}

