using System;
using Unity.Behavior;
using UnityEngine;
using Composite = Unity.Behavior.Composite;
using Unity.Properties;
using UnityEngine.InputSystem;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AbilitiesMenu", story: "[Unit] Chooses its next [Ability]", category: "Flow", id: "3ffc0ead7840b9fdd6ece80d60d5c0c9")]
public partial class AbilitiesMenuSequence : Composite
{
    [SerializeReference] public BlackboardVariable<AllyUnit> Unit;
    [SerializeReference] public BlackboardVariable<AbilityData> Ability;
    [SerializeReference] public BlackboardVariable<InputActionReference> Ability1Input;
    
    private InputAction _ability1InputAction;
    
    protected override Status OnStart() {
        Unit.Value.uiAbilities.enabled = true;

        _ability1InputAction = Ability1Input.Value?.ToInputAction();
        
        return Status.Running;
    }

    protected override Status OnUpdate() {
        
        if (_ability1InputAction.WasPerformedThisFrame()) {
            Ability.Value = Unit.Value.unitData.abilities[0];
            return Status.Success;
        }
        
        return Status.Waiting;
    }

    protected override void OnEnd() {
        Unit.Value.uiAbilities.enabled = false;
    }
}

