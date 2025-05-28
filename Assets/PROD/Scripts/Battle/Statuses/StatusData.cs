using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "status", menuName = "Ex32/StatusData", order = 0)]

public class StatusData : ScriptableObject {
    
    [Header("Values")]
    public bool isDebuff;
    public float Value;
    public int Duration;
    public bool isStackable;
    
    [Header("Effects")]
    public List<AbilityEffect> effectsOnTrigger;
    public AbilityTrigger triggerOnEvent;
    
    [Header("Display")]
    public string title;
    public Sprite icon;
    
    [TextArea(3, 5)]
    public string desc;
    public bool logDescOnProke;
    
    public void ApplyEffects(Unit caster, Unit target) {
        foreach (var effect in effectsOnTrigger) {
            effect.Apply(caster, target);
        }
    }
}
