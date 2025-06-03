using System;
using System.Collections.Generic;

public class StatusSystem : IInitializable<Unit> {
    
    public IEnumerable<StatusData> ActiveStatuses => _activeStatuses.Keys;
    
    public Action<StatusInstance> OnStatusChange;
    public Action<StatusInstance> OnStatusAdded;
    public Action<StatusInstance> OnStatusRemoved;
    
    private Dictionary<StatusData, StatusInstance> _activeStatuses;
    bool IInitializable<Unit>.Initialized { get; set; }
    private Unit _unit;

    public void Init(Unit unit) {
        _unit = unit;
        _activeStatuses = new Dictionary<StatusData, StatusInstance>();
    }

    public bool HasStatus(StatusData status) => _activeStatuses.ContainsKey(status);

    public int GetStackCount(StatusData status) {
        return _activeStatuses.TryGetValue(status, out var inst) ? inst.StackCount : 0;
    }

    public void ApplyStatus(StatusData status, Unit source, int stackCount = 1) {
        if (_activeStatuses.TryGetValue(status, out var instance)) {
            if (status.isStackable) {
                instance.AddStacks(stackCount, status.Duration);
                
                OnStatusChange?.Invoke(instance);
            }
            else {
                
                instance = new StatusInstance(status, source, status.Duration);
                _activeStatuses[status] = instance;
                
                OnStatusAdded?.Invoke(instance);
                OnStatusChange?.Invoke(instance);
            }
        }
        else {
            var newInstance = new StatusInstance(status, source, status.Duration, stackCount);
            _activeStatuses[status] = newInstance;
            
            OnStatusAdded?.Invoke(newInstance);
            OnStatusChange?.Invoke(newInstance);
        }
            
        if (status.triggerOnEvent == StatusTrigger.OnApplied) {
            status.ApplyEffects(source, _unit);
        }
        
    }

    public void TickStatuses() {
        var expiredStatuses = new List<StatusData>();

        foreach (var kvp in _activeStatuses) {
            kvp.Value.Tick();

            if (kvp.Value.IsExpired) {
                expiredStatuses.Add(kvp.Key);
                
                OnStatusChange?.Invoke(kvp.Value);
                OnStatusRemoved?.Invoke(kvp.Value);
            }
        }

        foreach (var status in expiredStatuses) {
            _activeStatuses.Remove(status);
            
            if (status.triggerOnEvent == StatusTrigger.OnApplied) {
                status.ApplyEffects(null, _unit);
            }
        }
    }

}
