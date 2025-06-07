using System;
using UnityEngine;

[Serializable]

public class APSystem 
{
    public event Action OnAPChanged;
    public event Action<int> OnAPSpent;
    public event Action<int> OnAPGained;
    public event Action OnAPDepleted;
    
    
    public int MaxAP {
        get => _maxAP;
        private set => _maxAP = value;
    }
    
    public int AP {
        get => _AP;
        private set => _AP = value;
    }
    
    public float NormalizedAP => AP / (float)MaxAP;
    
    [SerializeField] private int _maxAP;
    [SerializeField] private int _AP;

    public APSystem(int maxAP, int currentAP) {
        _maxAP = maxAP;
        _AP = currentAP;
    }

    public bool CanSpendAP(int amount) {
        return _AP > amount;
    }

    public bool TrySpendAP(int amount) {
        if (!CanSpendAP(amount)) return false;
        
        _AP = Mathf.Max(0, _AP - amount);
        
        OnAPSpent?.Invoke(amount);
        OnAPChanged?.Invoke();
        
        if(_AP == 0)
            OnAPDepleted?.Invoke();
        return true;
    }

    public void GiveAP(int amount) {
        _AP = Mathf.Min(_AP + amount, _maxAP);
        
        OnAPGained?.Invoke(amount);
        OnAPChanged?.Invoke();
    }
}
