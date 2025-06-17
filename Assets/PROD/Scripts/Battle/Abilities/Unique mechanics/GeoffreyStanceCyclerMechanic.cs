using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stance {
    public string name;
    [TextArea] public string description;
    public Sprite icon;
    
    [SerializeReference] public List<AbilityEffect> effects;
}

public class GeoffreyStanceCyclerMechanic : AUniqueMechanicSystem {
    
    [SerializeField] private List<Stance> stances;
    
    
    public override string Title => stances[_currentStanceIndex].name;
    public override string Description => stances[_currentStanceIndex].description;
    public override Sprite Icon => stances[_currentStanceIndex].icon;

    
    private int _currentStanceIndex = 0;
    
    protected override void OnTurnStarted(Unit unit) {
        if(unit != _unit) return;
        
        var stance = stances[_currentStanceIndex];
        
        foreach (var effect in stance.effects) {
            effect.Apply(unit, unit); // Ou autre logique
        }
        
        _currentStanceIndex = (_currentStanceIndex + 1) % stances.Count;
    }
}
