using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleData", menuName = "Ex32/BattleData")]
public class BattleData : ScriptableObject {
    public List<UnitData> enemies = new List<UnitData>();
    public List<UnitData> forcedAllyTeam = new List<UnitData>();
    
    public AudioClip battleSoundTtrack;
}
