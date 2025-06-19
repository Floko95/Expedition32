using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public class Stance {
    public string name;
    [TextArea] public string description;
    public Sprite icon;
    
    [SerializeReference] public List<AbilityEffect> effects;
}

public class GeoffreyStanceCyclerMechanic : AUniqueMechanicSystem {

    [SerializeField] private Stance WarmupStance;
    [SerializeField] private Stance ExerciseStance;
    [SerializeField] private Stance StretchingStance;
    
    [SerializeReference] private List<AbilityEffect> appliedIncrementalEffects;
    [SerializeField] private float efficiencyPerStage = 0.25f;
    
    public override string Title => _stances[_currentStanceIndex].name;
    public override string Description => _stances[_currentStanceIndex].description.Replace("<Efficiency>", Mathf.RoundToInt(efficiencyPerStage * Streak * 100).ToString(CultureInfo.InvariantCulture));
    public override Sprite Icon => _stances[_currentStanceIndex].icon;
    public Stance CurrentStance => _stances[_currentStanceIndex];
    
    public float CumulatedEfficiency { get; private set; }
    public int Streak { get; private set; }
    
    private List<Stance> _stances = new List<Stance>();
    private int _currentStanceIndex = 0;
    private bool _skipOnce;
    
    public override void Init(Unit data) {
        base.Init(data);
        _stances = new List<Stance>() { WarmupStance, ExerciseStance, StretchingStance };
        CumulatedEfficiency = 1f;
    }
    
    private void NextStance() {
        
        CumulatedEfficiency += efficiencyPerStage;
        foreach (var effect in appliedIncrementalEffects) {
            effect.Cancel(_unit, _unit);
            effect.Efficiency = CumulatedEfficiency;
            effect.Apply(_unit, _unit);
        }
        
        _currentStanceIndex = (_currentStanceIndex + 1) % _stances.Count;
        Streak++;
        
        onMechanicUpdated?.Invoke();
    }

    public void ResetChain() {
        _currentStanceIndex = 0;
        Streak = 0;
        CumulatedEfficiency = 1f;
        
        foreach (var effect in appliedIncrementalEffects) {
            effect.Cancel(_unit, _unit);
            effect.Efficiency = CumulatedEfficiency;
        }
    }

    public void SkipNextAbilityUsed() {
        _skipOnce = true;
    }
    
    protected override void OnAbilityUsed(AbilityData ability) {
        base.OnAbilityUsed(ability);
        
        if (_skipOnce) {
            _skipOnce = false;
            NextStance();
        }
        
        if (ability == _unit.unitData.attackAbility && CurrentStance == WarmupStance) {
            NextStance();
        } else if (_unit.Abilities.Contains(ability) && CurrentStance == ExerciseStance) {
            NextStance();
        } else if (ability is UseItemAbility && CurrentStance == StretchingStance) {
            NextStance();
        }
        else {
            ResetChain();
        }
    }
}
