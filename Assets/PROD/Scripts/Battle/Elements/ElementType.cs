using UnityEngine;

public enum ElementType {
   Physical,
   Fire,
   Lightning,
   Earth,
   Ice,
   Light,
   Dark,
   Void,
}

public enum ElementReaction
{
   Normal,      // 1x
   Weak,        // 2x
   Resistant,   // 0.5x
   Immune,      // 0x
   Absorb       // -1x (soin)
}

[System.Serializable]
public class ElementInteraction {
   public ElementType type;
   public ElementReaction reaction;
}
