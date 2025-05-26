using System;

namespace OSLib.StatSystem
{
    public enum StatModType {
        Flat = 100,
        PercentAdd = 200,
        PercentMult = 300,
    }

    [Serializable]
    public class StatModifier {
        public float value;
        public StatModType type;
        public int Order;
        public object Source;

        public StatModifier(float value, StatModType modType, int order, object source) {
            this.value = value;
            this.type = modType;
            this.Order = order;
            this.Source = source;
        }

        public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) { }
        public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) { }
        public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }
    }
}
