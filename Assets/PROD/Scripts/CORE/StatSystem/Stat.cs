using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

namespace OSLib.StatSystem {

    [Serializable]
    public class Stat {

        public float baseValue;
        public virtual float Value {
            get {
                if(this._isDirty || _lastBaseValue != this.baseValue) {
                    this._lastBaseValue = this.baseValue;
                    _value = this.CalculateFinalValue();
                    this._isDirty = false;
                }
                return _value;
            }
        }
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;
        public event Action onModifierAdded;
        public event Action onModifierRemoved;

        protected readonly List<StatModifier> _statModifiers;
        protected float _value;
        protected bool _isDirty = true;
        protected float _lastBaseValue;

        public Stat() {
            this._statModifiers = new List<StatModifier>();
            this.StatModifiers = this._statModifiers.AsReadOnly();
        }
        public Stat(float baseValue) : this() {
            this.baseValue = baseValue;
        }

        public virtual void AddModifier(StatModifier mod) {
            this._isDirty = true;
            _statModifiers.Add(mod);
            _statModifiers.Sort(this.CompareModifierOrder);
            this.onModifierAdded?.Invoke();
        }

        public virtual bool RemoveModifier(StatModifier mod) {
            if(_statModifiers.Remove(mod)) {
                this._isDirty = true;
                this.onModifierRemoved?.Invoke();
                return true;
            }
            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source) {
            bool didRemove = false;
            for(int i = _statModifiers.Count - 1; i>= 0; i--) {
                if (_statModifiers[i].Source == source) {
                    _isDirty = true;
                    didRemove = true;
                    _statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

        protected virtual float CalculateFinalValue() {
            float finalValue = baseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < _statModifiers.Count; i++) {
                StatModifier mod = _statModifiers[i];
                if(mod.type == StatModType.Flat) {
                    finalValue += _statModifiers[i].value;
                } else if(mod.type == StatModType.PercentAdd) {
                    sumPercentAdd += mod.value; // Start adding together all modifiers of this type
 
                    // If we're at the end of the list OR the next modifer isn't of this type
                    if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].type != StatModType.PercentAdd) {
                        finalValue *= 1 + sumPercentAdd;                                // Multiply the sum with the "finalValue", like we do for "PercentMult" modifiers
                        sumPercentAdd = 0;                                              // Reset the sum back to 0
                    }
                } else if(mod.type == StatModType.PercentMult) {
                    finalValue *= 1 + mod.value;
                }
            }
            
            return (float)Math.Round(finalValue, 4);
        }

        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b) {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0; // if (a.Order == b.Order)
        }
    }
}
