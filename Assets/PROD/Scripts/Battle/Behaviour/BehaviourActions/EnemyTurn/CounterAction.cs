using System;
using System.Collections.Generic;
using System.Linq;
using BitDuc.Demo;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CounterAction", story: "[Targets] counters.", category: "Action", id: "031a50f6f20ab5de752a0cf82c69dd8a")]
public partial class CounterAction : Action
{
    [SerializeReference] public BlackboardVariable<AbilityData> abilitySource;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Targets;
    
    private IDisposable _comboListener;
    private bool _hasCinematicEnded;
    private List<Unit> _targets;
    
    //WIP
    protected override Status OnStart()
    {
        _targets = Targets.Value.Select(u => u.GetComponent<Unit>()).Where(u => u != null).ToList();

        switch (abilitySource.Value.targetMode) {
            case AbilityTargetMode.SelectTarget:
                break;
            case AbilityTargetMode.AllEnemies:
                break;
            default:
                break;
        }
        
        
        return Status.Running;
    }

    private void OnTimelineUpdate() { }

    private void OnTimelineComplete() {
        _hasCinematicEnded = true;
        
    }
    
    private void HandleComboWindow(ComboWindow window) {
        if (abilitySource.Value == null) return;
        
        foreach (var target in _targets) {
           // Ability.Value.ApplyEffects(Unit.Value, target);
        }
    }
    
    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd() {
        _comboListener.Dispose();
    }
}

