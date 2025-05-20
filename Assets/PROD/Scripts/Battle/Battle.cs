using System;
using System.Collections.Generic;

[Serializable]
public class Battle
{
    public List<Unit> Units = new List<Unit>();
    public List<Unit> Allies = new List<Unit>();
    public List<Unit> Enemies = new List<Unit>();
}

