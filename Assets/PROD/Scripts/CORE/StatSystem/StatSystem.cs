using System;
using System.Collections.Generic;
using Sirenix.Serialization;

namespace OSLib.StatSystem
{
    public interface IHaveStats {
        public StatSystem GetStatSystem();
    }

    public enum StatType { //TODO define your stats here
        Health,
        ATK,
        DEF,
        SPD,
        CRIT,
    }

    [Serializable]
    public class StatSystem
    {
        [OdinSerialize] public Dictionary<StatType, Stat> stats;

        public StatSystem(Dictionary<StatType, float> baseStats) {
            stats = new Dictionary<StatType, Stat>();
            
            foreach (var stat in baseStats) {
                stats.Add(stat.Key, new Stat(stat.Value));
            }
        }
    }
}
