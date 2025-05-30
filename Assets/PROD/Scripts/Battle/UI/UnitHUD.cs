using UnityEngine;
using UnityEngine.UI;

public class UnitHUD : MonoBehaviour, IInitializable<Unit>
{
    [SerializeField] private HealthBarUI healthBarUI;
    [SerializeField] private Slider APSlider; //temp
    [SerializeField] private Image iPortrait;
    
    
    bool IInitializable<Unit>.Initialized { get; set; }

    private void Awake() {
        Init(GetComponentInParent<Unit>());
    }

    public void Init(Unit unit) {
        if(unit == null) return;
        
        healthBarUI.SetHealthSystem(unit.HealthSystem);

        
        if (APSlider) {
            APSlider.minValue = 0;
            APSlider.maxValue = unit.MaxEnergy;
            APSlider.value = unit.Energy;
        }
        
        if(iPortrait)
            iPortrait.sprite = unit.unitData.portrait;
    }
    
}
