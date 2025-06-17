using TMPro;
using UnityEngine;

public class CharlesStanceSwitcherUI : MonoBehaviour, IInitializable<CharlesStanceSwitcherMechanic> {

    bool IInitializable<CharlesStanceSwitcherMechanic>.Initialized { get; set; }
    
    public void Init(CharlesStanceSwitcherMechanic data) {
        
    }
}
