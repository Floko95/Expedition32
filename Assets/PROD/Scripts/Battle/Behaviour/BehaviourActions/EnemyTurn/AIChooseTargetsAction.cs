using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AIChooseTargets", story: "AI [Unit] Chooses the next [Targets]", category: "Action", id: "761f0174f7b60094ece77b04c7156290")]
public partial class AIChooseTargetsAction : Action
{
    [SerializeReference] public BlackboardVariable<BattleManager> BattleManager;
    [SerializeReference] public BlackboardVariable<Unit> Caster;
    [SerializeReference] public BlackboardVariable<AbilityData> UsedAbility;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Targets;

    //Enemy choose its next target
    protected override Status OnStart() {
        var aliveAllies = BattleManager.Value.Battle.Allies.Where(a => a.IsAlive).Select(u => u.gameObject).ToList();
        var aliveEnemies = BattleManager.Value.Battle.Enemies.Where(e => e.IsAlive).Select(u => u.gameObject).ToList();
        
        switch (UsedAbility.Value.targetMode) {
            case AbilityTargetMode.None:
                break;
            case AbilityTargetMode.CharacterSelf:
                Targets.Value = new List<GameObject> {Caster.Value.gameObject};
                break;
            case AbilityTargetMode.Ally:
                Targets.Value = new List<GameObject> { aliveEnemies[Random.Range(0, aliveEnemies.Count)] };
                break;
            case AbilityTargetMode.AllAllies:
                Targets.Value = aliveEnemies;
                break;
            case AbilityTargetMode.SelectTarget:
                Targets.Value = new List<GameObject> { aliveAllies[Random.Range(0, aliveAllies.Count)] };
                break;
            case AbilityTargetMode.AllEnemies:
                Targets.Value = aliveAllies;
                break;
            default:
                throw new ArgumentOutOfRangeException();
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

