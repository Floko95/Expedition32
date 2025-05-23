using UnityEngine;
using UnityEngine.UI;

public class UnitHUD : MonoBehaviour, IInitializable<Unit>
{
    [SerializeField] private HealthBarUI healthBarUI;
    [SerializeField] private Slider APSlider; //temp
    [SerializeField] private Image iPortrait;
    
    bool IInitializable<Unit>.Initialized { get; set; }
    
    public void Init(Unit unit) {
        healthBarUI.SetHealthSystem(unit.HealthSystem);
        
        APSlider.minValue = 0;
        APSlider.maxValue = unit.MaxEnergy;
        APSlider.value = unit.Energy;
        
        iPortrait.sprite = unit.unitData.portrait;
    }
}
