using UnityEngine;
using UnityEngine.UI;

public class UnitHUD : MonoBehaviour, IInitializable<Unit>
{
    [SerializeField] private HealthBarUI healthBarUI;
    [SerializeField] private APBarUI apBarUI;
    
    [SerializeField] private Image iPortrait;
    
    
    bool IInitializable<Unit>.Initialized { get; set; }

    private void Awake() {
        Init(GetComponentInParent<Unit>());
    }

    public void Init(Unit unit) {
        if(unit == null) return;
        
        healthBarUI.SetHealthSystem(unit.HealthSystem);
        if(apBarUI)
            apBarUI.Init(unit);
        
        if(iPortrait)
            iPortrait.sprite = unit.unitData.portrait;
    }
}
