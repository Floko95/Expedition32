using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TurnQueue {

    private static int THRESHOLD = 500;
    
    public Unit CurrentTurn => turnQueue.Count > 0 ? turnQueue[0] : null;
    public List<Unit> turnQueue = new List<Unit>();
    public UnityAction onTurnsUpdated;

    private int Capacity = 5;
    
    private List<Unit> _units;
    private bool _skipFirstTurn = true;
    
    public TurnQueue(List<Unit> units) {
        _units = units;
        UpdateTurnQueue();
    }

    public List<Unit> PeekNextTurns(int count) {
        return turnQueue.Take(Mathf.Min(Capacity, count)).ToList();
    }
    
    public Unit GetNext() {
        if (turnQueue.Count == 0) return null;

        if (_skipFirstTurn) {
            _skipFirstTurn = false;
            return CurrentTurn;
        }
                
        UpdateTurnQueue();
        return CurrentTurn;
    }

    public void UpdateTurnQueue() {
        turnQueue.Clear();
            
        for (int i = 0; i < Capacity; i++) {
            foreach (var unit in _units) {
                unit.Initiative += Mathf.RoundToInt(unit.SPD);
            }

            var sortedByInitiative = SortByInitiative(_units).Where(u => u.Initiative >= THRESHOLD).ToList();
            var fastest = sortedByInitiative.FirstOrDefault();
            if(fastest == null) continue;
            
            turnQueue.Add(fastest);
            
            fastest.Initiative = 0;
        }
        
        onTurnsUpdated?.Invoke();
    }
    
    private IEnumerable<Unit> SortByInitiative(IEnumerable<Unit> units, int initiativeBonus = 0) {
        return units
            .Where(u => u.IsAlive)
            .OrderByDescending(u => u.Initiative + initiativeBonus);
    }
    
    public class SimUnit {
        public Unit original;
        public float SPD;
    }
}
