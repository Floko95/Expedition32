using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class Unit : MonoBehaviour, ITargetable {

    public static int MAP_AP = 9;
        
    [SerializeField] public Canvas WorldUI;
    [SerializeField] public PlayableDirector playableDirector;
        
    public HealthSystem HealthSystem { get; private set; }

    public int Energy { get; private set; }
    public int MaxEnergy { get; private set; }

    public int Shield { get; private set; }
    public int Speed { get; private set; }
    
    public int Initiative { get; set; }
    
    public bool IsAlive => HealthSystem.IsAlive;
    public bool isEnemy => unitData && unitData.isEnemy;
    
    public UnitData unitData;
    
    public List<AbilityData> Abilities { get; private set; } = new List<AbilityData>();

    protected virtual void Awake() {
        if(WorldUI)
            WorldUI.enabled = false;
        
        if (unitData != null) {
            Init(unitData);
        }
    }

    public virtual void Init(UnitData unitData) {
        HealthSystem = new HealthSystem(unitData.maxHealth);
        
        Speed = unitData.speed;
        Initiative = unitData.speed;
        
        Energy = unitData.energy;
        MaxEnergy = MAP_AP;
        
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
