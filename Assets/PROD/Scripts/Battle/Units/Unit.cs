using System;
using System.Collections.Generic;
using BitDuc.EnhancedTimeline.Timeline;
using OSLib.StatSystem;
using UnityEngine;

public class Unit : MonoBehaviour, ITargetable, IHaveStats {

    public static int MAX_AP = 9;
        
    [SerializeField] public Canvas WorldUI;
    [SerializeField] private Canvas WorldHUD;
    
    [SerializeField] public EnhancedTimelinePlayer playableDirector;
    [SerializeField] public Animator animator;
    [SerializeField] private Transform vCamTarget;
    [SerializeField] public UnitData unitData;
    Transform IHaveBehaviour.transform => vCamTarget;
    
    public HealthSystem HealthSystem { get; private set; }
    public APSystem APSystem { get; private set; }
    public StatSystem GetStatSystem() => _statSystem;
    public StatusSystem StatusSystem { get; private set; }
    
    public int Shield { get; private set; }

    public float ATK => _statSystem.stats[StatType.ATK].Value;
    public float DEF => _statSystem.stats[StatType.DEF].Value;
    public float CRIT => _statSystem.stats[StatType.CRIT].Value;
    public float SPD => _statSystem.stats[StatType.SPD].Value;
    
    public int Initiative { get; set; }
    
    public bool IsAlive => HealthSystem.IsAlive;
    public bool isEnemy => unitData && unitData.isEnemy;

    public static Action<Unit, int> OnAnyUnitRegainedAP;
    
    public List<AbilityData> Abilities { get; private set; } = new List<AbilityData>();
    private StatSystem _statSystem;
    
    protected virtual void Awake() {
        OnUntargeted();
        
        if (unitData != null) {
            Init(unitData);
        }
    }

    protected virtual void Start() {
        HealthSystem.OnDamaged += OnDamaged;
        HealthSystem.OnDead += OnDeath;
        APSystem.OnAPGained += OnAPGained;
    }

    protected virtual void OnDestroy() {
        HealthSystem.OnDamaged -= OnDamaged;
        HealthSystem.OnDead -= OnDeath;
        APSystem.OnAPGained -= OnAPGained;
    }
    
    public virtual void Init(UnitData unitData) {
        _statSystem = new StatSystem(unitData.stats);
        HealthSystem = new HealthSystem(_statSystem.stats[StatType.Health].Value);
        APSystem = new APSystem(MAX_AP, unitData.energy);
        
        StatusSystem = new StatusSystem();
        StatusSystem.Init(this);
        
        Initiative = Mathf.RoundToInt(SPD);
        
        name = unitData.unitName;
        
        Abilities = unitData.abilities;
    }

    private void OnDamaged() {
        animator.Play("Hit");
    }
    
    private void OnDeath() {
        animator.SetBool("IsDead", true);
    }
    
    private void OnAPGained(int amount) {
        OnAnyUnitRegainedAP.Invoke(this, amount);
    }
    
    public void OnTargeted() {
        if(WorldHUD)
            WorldHUD.enabled = true;
    }

    public void OnUntargeted() {
        if(WorldHUD)
            WorldHUD.enabled = false;
    }
}
