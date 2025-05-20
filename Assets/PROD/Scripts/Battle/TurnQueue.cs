using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class TurnQueue
{
    public Unit CurrentTurn;
    public List<Unit> turnQueue = new List<Unit>();

    public UnityAction onTurnsUpdated;
    
    private List<Unit> _units;
    
    public TurnQueue(List<Unit> units) {
        _units = units;
        
        turnQueue.Clear();
        Sort();
        
        onTurnsUpdated?.Invoke();
    }

    public List<Unit> PeekNextTurns(int count) {
        return turnQueue.Take(count).ToList();
    }
    
    public Unit Next() {
        if (turnQueue.Count == 0) return null;
        CurrentTurn = turnQueue[0];
        
        onTurnsUpdated?.Invoke();
        
        return turnQueue[0];
    }
    
    private void Sort() {
        turnQueue = _units
            .Where(u => u.IsAlive)
            .OrderByDescending(u => u.Speed)
            .ToList();
    }
}
