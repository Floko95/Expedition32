using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ability", menuName = "Ex32/UseItemAbilityData", order = 2)]
public class UseItemAbility : AbilityData
{
    [SerializeField] private ItemData usedItem;
    
    public override string title => usedItem ? usedItem.name : base.title;
    public override string desc => usedItem? usedItem.desc : base.desc;
    public override List<AbilityEffect> effects => usedItem ? usedItem.onUseEffects : base.effects;
    

    public override void ApplyEffects(Unit caster, Unit target) {
        usedItem.Use(caster, target); 
    }

    private void OnValidate() {
        _title = usedItem ? usedItem.name : base.title;
        _desc = usedItem ? usedItem.desc : base.desc;
        _effects = usedItem ? usedItem.onUseEffects : base.effects;
    }
}
