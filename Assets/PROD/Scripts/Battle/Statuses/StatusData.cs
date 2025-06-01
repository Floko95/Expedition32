using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "status", menuName = "Ex32/StatusData", order = 0)]

public class StatusData : ScriptableObject {
    
    [Header("Values")]
    public bool isDebuff;
    public int Duration;
    public bool isStackable;
    
    [Header("Effects")]
    public List<AbilityEffect> effectsOnTrigger;
    public StatusTrigger triggerOnEvent;
    
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

public enum StatusTrigger
{
    None = 0,
    
    OnApplied = 10,
    OnExpired = 11,
    
    StartOfOwnerTurn = 20,       //Every turn
    EndOfOwnerTurn = 22,         //Every turn
    
    OnOwnerDeath = 40,       //When unit is dying
    OnOwnerDamaged = 42,     //When unit is damaged

    BattleStart = 50,
    BattleEnd = 52,
}
