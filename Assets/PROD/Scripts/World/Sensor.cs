using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Sensor : MonoBehaviour
{
    [SerializeField] private float sensorRadius;
    [SerializeField] private List<string> targetTags;
    [SerializeField] private bool debug;
    
    public UnityEvent OnTargetDetected = new UnityEvent();
    
    readonly List<Transform> detectedTargets = new List<Transform>(10);
    private SphereCollider _sphereCollider;

    private void Start() {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
        _sphereCollider.radius = sensorRadius;
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, sensorRadius);
        foreach (var c in colliders) {
            ProcessDetectedTargets(c, t => detectedTargets.Add(t));
        }
    }

    private void OnTriggerEnter(Collider other) {
        ProcessDetectedTargets(other, t => detectedTargets.Add(t));
    }

    private void OnTriggerExit(Collider other) {
        ProcessDetectedTargets(other, t => detectedTargets.Remove(t));
    }

    private void ProcessDetectedTargets(Collider other, Action<Transform> action) {
        var validTargets = targetTags.Where(other.CompareTag).ToList();
        foreach (var tag in validTargets) {
            action(other.transform);
        }

        if (validTargets.Count > 0) {
            OnTargetDetected?.Invoke();
        }
    }

    public Transform GetClosestDetectedTarget(string tag) {
        if (detectedTargets.Count == 0) return null;
        
        Transform closest = null;
        float closestDist = float.MaxValue;

        foreach (Transform t in detectedTargets) {
            if(t.CompareTag(tag) == false) continue;
            float dist = Vector3.Distance(transform.position, t.position);
            
            if (!(dist < closestDist)) continue;
            
            closestDist = dist;
            closest = t;
        }

        return closest;
    }
    
    private void OnDrawGizmos() {
        if(debug == false) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sensorRadius);
    }
}
