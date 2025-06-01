using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "ability", menuName = "Ex32/AbilityData", order = 1)]
public class AbilityData : ScriptableObject
{
    [Title("Display")]
    [SerializeField] public string title;
    [TextArea(5, 7)] public string desc;
    [SerializeField] public Sprite icon;
    
    [Title("Target")]
    [SerializeField] public AbilityTargetMode targetMode;               //WHO is targeted?
    
    [Title("Effect")]
    [SerializeField] public int costAP;
    [SerializeReference] public List<AbilityEffect> effects;              //WHAT this does?
    [SerializeReference] public AbilityTrigger effectTriggerEvent = AbilityTrigger.OnPlay;              //WHEN?
    
    [Title("Visuals")]
    [SerializeField] public TimelineAsset timeline;


    public void ApplyEffects(Unit caster, Unit target) {
        foreach (var effect in effects) {
            effect.Apply(caster, target);
        }
    }
}

public enum AbilityTrigger
{
    None = 0,
    
    OnPlay = 10,  //When played

    StartOfTurn = 20,       //Every turn
    EndOfTurn = 22,         //Every turn
    
    OnDeath = 40,       //When unit is dying
    OnDamaged = 42,     //When unit is damaged

    BattleStart = 50,
    BattleEnd = 52,
}
