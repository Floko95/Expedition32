using System;
using Unity.Behavior;
using UnityEngine;
using Composite = Unity.Behavior.Composite;
using Unity.Properties;
using UnityEngine.InputSystem;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MainMenu", story: "Displays [Unit] 's main menu", category: "Flow", id: "ab75ebf7f2d9697c0158f5411ba0628e")]
public partial class MainMenuSequence : Composite
{
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public Node AbilitiesMenu;
    [SerializeReference] public Node ItemsMenu;
    [SerializeReference] public BlackboardVariable<InputActionAsset> InputActionAsset;
    
    private InputAction _abilitySelectionInput;
    private InputAction _itemSelectionInput;
    private AllyUnit _allyUnit;
    
    protected override Status OnStart() {
        _abilitySelectionInput = InputActionAsset.Value?.FindAction("Abilities", true);
        _itemSelectionInput = InputActionAsset.Value?.FindAction("Items", true);
        
        _allyUnit = Unit.Value as AllyUnit;
        _allyUnit.WorldUI.enabled = true;
        _allyUnit.uiPrompt.enabled = true;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_abilitySelectionInput != null && _abilitySelectionInput.WasPerformedThisFrame()) {
            return StartNode(AbilitiesMenu);
        } else if (_itemSelectionInput != null && _itemSelectionInput.WasPerformedThisFrame()) {
            return StartNode(ItemsMenu);
        }
        
        return Status.Waiting;
    }

    protected override void OnEnd() {
        _allyUnit.WorldUI.enabled = false;
        _allyUnit.uiPrompt.enabled = false;
    }
}

