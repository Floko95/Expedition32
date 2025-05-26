using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Ex32/UnitData")]
public class UnitData : ScriptableObject
{
    [SerializeField] public string unitName;
    [SerializeField] public Sprite portrait;
    [SerializeField] public Unit prefab;
    [SerializeField] public bool isEnemy;

    [Title("Stats")]
    
    [SerializeField] public int maxHealth;
    [SerializeField] public int energy;
    [SerializeField] public int ATK;
    [SerializeField] public int DEF;
    [SerializeField] public int CRIT;
    
    [SerializeField] public int speed;

    [SerializeField] public List<AbilityData> abilities;
}
