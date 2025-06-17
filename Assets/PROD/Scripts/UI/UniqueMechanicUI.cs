using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UniqueMechanicUI : MonoBehaviour, IInitializable<AUniqueMechanicSystem> {
    
    [SerializeField] private TextMeshProUGUI titleTmp;
    [SerializeField] private TextMeshProUGUI descriptionTmp;
    [SerializeField] private Image icon;
    
    bool IInitializable<AUniqueMechanicSystem>.Initialized { get; set; }
    
    private AUniqueMechanicSystem _system;
    
    public void Init(AUniqueMechanicSystem system) {
        _system = system;
        UpdateUI();
    }

    public void UpdateUI() {
        titleTmp.text = _system.Title;
        descriptionTmp.text = _system.Description;
        icon.sprite = _system.Icon;
    }
}
