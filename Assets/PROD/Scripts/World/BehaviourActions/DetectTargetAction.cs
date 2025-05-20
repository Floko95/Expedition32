using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DetectTarget", story: "[Self] detects [Target]", category: "Action", id: "dc1951d43b71dd30ecd841807b85eb3e")]
public partial class DetectTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    
    private NavMeshAgent _agent;
    private Sensor _sensor;
    
    protected override Status OnStart() {
        _agent = Self.Value.GetComponent<NavMeshAgent>();
        _sensor = Self.Value.GetComponentInChildren<Sensor>();
        
        return Status.Running;
    }

    protected override Status OnUpdate() {
        var target = _sensor.GetClosestDetectedTarget("Player");
        if (target == null) return Status.Running;
        
        Target.Value = target.gameObject;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

