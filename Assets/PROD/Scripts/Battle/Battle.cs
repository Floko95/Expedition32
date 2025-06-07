using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Battle
{
    public List<Unit> Units = new List<Unit>();
    public List<Unit> Allies = new List<Unit>();
    public List<Unit> Enemies = new List<Unit>();
    
    public List<Unit> AliveAllies => Allies.Where(a => a.IsAlive).ToList();
    public List<Unit> DeadAllies => Allies.Where(a => !a.IsAlive).ToList();
    public List<Unit> AliveEnemies => Enemies.Where(e => e.IsAlive).ToList();
}

