using System.Collections.Generic;
using BitDuc.EnhancedTimeline.Timeline;
using OSLib.StatSystem;
using UnityEngine;

public class Unit : MonoBehaviour, ITargetable, IHaveStats {

    public static int MAX_AP = 9;
        
    [SerializeField] public Canvas WorldUI;
    [SerializeField] public EnhancedTimelinePlayer playableDirector;
    [SerializeField] public Animator animator;
    
    public HealthSystem HealthSystem { get; private set; }

    public StatSystem GetStatSystem() => _statSystem;
    
    public int Energy { get; private set; }
    public int MaxEnergy { get; private set; }

    public int Shield { get; private set; }

    public float ATK => _statSystem.stats[StatType.ATK].Value;
    public float DEF => _statSystem.stats[StatType.DEF].Value;
    public float CRIT => _statSystem.stats[StatType.CRIT].Value;
    public float SPD => _statSystem.stats[StatType.SPD].Value;
    
    public int Initiative { get; set; }
    
    public bool IsAlive => HealthSystem.IsAlive;
    public bool isEnemy => unitData && unitData.isEnemy;
    
    public UnitData unitData;
    
    public List<AbilityData> Abilities { get; private set; } = new List<AbilityData>();
    private StatSystem _statSystem;
    
    protected virtual void Awake() {
        if(WorldUI)
            WorldUI.enabled = false;
        
        if (unitData != null) {
            Init(unitData);
        }
    }

    public virtual void Init(UnitData unitData) {
        _statSystem = new StatSystem(unitData.stats);
        HealthSystem = new HealthSystem(_statSystem.stats[StatType.Health].Value);
        
        Initiative = Mathf.RoundToInt(SPD);
        
        Energy = unitData.energy;
        MaxEnergy = MAX_AP;
        
        Abilities = unitData.abilities;
    }

    public void OnTargeted() {
        if(WorldUI)
            WorldUI.enabled = true;
    }

    public void OnUntargeted() {
        if(WorldUI)
            WorldUI.enabled = false;
    }
}
