using System;
using Unity.Behavior;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ReturnToWorldAction", story: "Load World Scene [WorldScene]", category: "Action", id: "e512e289e252bc5b179877505438973f")]
public partial class ReturnToWorldAction : Action {
    [SerializeReference] public BlackboardVariable<string> WorldScene;
    
    protected override Status OnStart() {
        var stm = Toolbox.Get<SceneTransitionManager>();
        stm.LoadScene(WorldScene);
        return Status.Running;
    }

    protected override Status OnUpdate() {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

