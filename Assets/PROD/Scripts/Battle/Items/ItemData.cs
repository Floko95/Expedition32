using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] public string name;
    [TextArea(5, 7)] public string desc;
    
    [SerializeReference] public List<AbilityEffect> onUseEffects;
    [SerializeField] public int maxAmount;

    public void Use(Unit caster, Unit target) {
        
        foreach (var effect in onUseEffects) {
            effect.Apply(caster, target);
        }
    }
}
