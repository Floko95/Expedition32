using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Ex32/UnitData")]
public class UnitData : ScriptableObject
{
    [SerializeField] public string unitName;
    [SerializeField] public Sprite portrait;
    [SerializeField] public Unit prefab;
    [SerializeField] public bool isEnemy;

    [SerializeField] public int maxHealth;
    [SerializeField] public int energy;
    [SerializeField] public int speed;

    [SerializeField] public List<AbilityData> abilities;
}
