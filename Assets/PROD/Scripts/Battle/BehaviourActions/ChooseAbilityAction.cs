using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChooseAbilityAction", story: "[Unit] chooses its next [Ability] of Type [TargetMode]", category: "Action", id: "025cac6d3ca33d7f343bfb30b0cbc802")]
public partial class ChooseAbilityAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<AbilityData> Ability;
    [SerializeReference] public BlackboardVariable<AbilityTargetMode> TargetMode;
    
    // pick abilities in sequence by default
    private int _abilityIndex;
    
    protected override Status OnStart()
    {
        if(Unit.Value.unitData.abilities.Count == 0) return Status.Failure;

        Ability.Value = Unit.Value.unitData.abilities[_abilityIndex % Unit.Value.unitData.abilities.Count];
        TargetMode.Value = Ability.Value.targetMode;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
        _abilityIndex++;
    }
}

