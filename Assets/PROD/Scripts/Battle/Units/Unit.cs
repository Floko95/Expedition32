using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour, ITargetable {

    [SerializeField] private Canvas WorldUI;
    
    public HealthSystem HealthSystem { get; private set; }

    public int Energy { get; private set; }
    public int MaxEnergy { get; private set; }

    public int Shield { get; private set; }
    public int Speed { get; private set; }

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
        Energy = 0;
        Speed = unitData.speed;
        MaxEnergy = unitData.energy;
        Abilities = unitData.abilities;
    }

    public virtual IEnumerator ExecuteTurn() {
        yield return null;
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
