using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "ability", menuName = "Ex32/AbilityData", order = 1)]
public class AbilityData : ScriptableObject
{
    [Header("Text")]
    [SerializeField] public string title;
    [TextArea(5, 7)] public string desc;
    
    [Title("Trigger")]
    [SerializeField] public AbilityTrigger trigger;             //WHEN does the ability trigger?
    
    [FormerlySerializedAs("target")]
    [Title("Target")]
    [SerializeField] public AbilityTargetMode targetMode;               //WHO is targeted?
    
    [Header("Effect")]
    [SerializeField] public EffectData[] effects;              //WHAT this does?
    [SerializeField] public StatusData[] status;               //Status added by this ability
    [SerializeField] public int value;
    
    [Header("Visuals")]
    [SerializeField] public TimelineAsset timeline;
    
}

public enum AbilityTrigger
{
    None = 0,

    Ongoing = 2,  //Always active (does not work with all effects)
    OnPlay = 10,  //When played

    StartOfTurn = 20,       //Every turn
    EndOfTurn = 22,         //Every turn

    OnDeath = 40,       //When character is dying
    OnDamaged = 42,     //When character is damaged

    BattleStart = 50,
    BattleEnd = 52,
}
