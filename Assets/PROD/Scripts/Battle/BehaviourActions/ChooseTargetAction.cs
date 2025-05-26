using System;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChooseTarget", story: "Chooses the next [Target]", category: "Action", id: "761f0174f7b60094ece77b04c7156290")]
public partial class ChooseTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<BattleManager> BattleManager;
    [SerializeReference] public BlackboardVariable<Unit> Target;

    //Enemy choose its next target
    protected override Status OnStart() {
        var aliveAllies =  BattleManager.Value.Battle.Allies.Where(a => a.IsAlive).ToList();
        Target.Value = aliveAllies[Random.Range(0, aliveAllies.Count)];
        
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

