using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "ability", menuName = "Ex32/AbilityData", order = 1)]
public class AbilityData : ScriptableObject
{
    [Title("Display"), PropertySpace(20)]
    [FormerlySerializedAs("title")] [SerializeField] public string _title;
    [FormerlySerializedAs("desc")] [TextArea(5, 7)] public string _desc;
    [SerializeField, PreviewField(ObjectFieldAlignment.Left), PropertySpace(5,5)] public Sprite icon;
    
    [Title("Target"), PropertySpace(20)]
    [SerializeField] public AbilityTargetMode targetMode;               //WHO is targeted?
    [SerializeField, EnumToggleButtons] public DodgeTypeEnum dodgeMode = DodgeTypeEnum.Undodgeable;               //How to dodge?
    
    [Title("Effect"), PropertySpace(20)]
    [SerializeField, ProgressBar(0, 9 ,b:175, g:15, DrawValueLabel = true, Segmented = true, ValueLabelAlignment = TextAlignment.Center, Height = 20)]
    public int costAP;
    
    [SerializeReference, PropertySpace(5,5)] public List<AbilityEffect> _effects;              //WHAT this does?
    [SerializeReference] public AbilityTrigger effectTriggerEvent = AbilityTrigger.OnPlay;              //WHEN?
    
    [Title("Visuals"), PropertySpace(20)]
    [SerializeField] public TimelineAsset timeline;

    public virtual string title => _title;
    public virtual string desc => _desc;
    public virtual List<AbilityEffect> effects => _effects;
    
    public virtual void ApplyEffects(Unit caster, Unit target) {
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
