using TMPro;
using UnityEngine;

public class AbilityUI : MonoBehaviour, IInitializable<AbilityData>
{
    [SerializeField] private TextMeshProUGUI titleTmp;
    [SerializeField] private TextMeshProUGUI descriptionTmp;
    [SerializeField] private TextMeshProUGUI costTmp;
    
    bool IInitializable<AbilityData>.Initialized { get; set; }

    public void Init(AbilityData data) {
        titleTmp.text = data.title;
        descriptionTmp.text = data.desc;
        costTmp.text = data.costAP.ToString();
        
    }
}
