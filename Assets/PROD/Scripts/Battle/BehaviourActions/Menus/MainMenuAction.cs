using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MainMenuAction", story: "Set [Unit] 's main menu to [Active]", category: "Action/UI", id: "0cfbf86ea16e47965a34d0fb2a29ccda")]
public partial class MainMenuAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<bool> Active;
    
    private AllyUnit _allyUnit;
    
    protected override Status OnStart() {
        _allyUnit = Unit.Value as AllyUnit;
        _allyUnit.WorldUI.enabled = true;
        
        _allyUnit.uiPrompt.enabled = Active.Value;
        
        return Status.Running;
    }

    protected override Status OnUpdate() {
        return Status.Success;
    }

    protected override void OnEnd() {
    }
}

