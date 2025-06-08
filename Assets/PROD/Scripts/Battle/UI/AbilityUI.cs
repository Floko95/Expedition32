using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class AbilityUI : AnInteractable, IInitializable<AbilityData>
{
    [SerializeField] private TextMeshProUGUI titleTmp;
    [SerializeField] private TextMeshProUGUI descriptionTmp;
    [SerializeField] private TextMeshProUGUI costTmp;

    [Title("Feedbacks")]
    [SerializeField] private MMF_Player defaultStateFeel;
    [SerializeField] private MMF_Player oomStateFeel;
    
    public AbilityData Data { get; private set; }
    
    bool IInitializable<AbilityData>.Initialized { get; set; }
    
    public void Init(AbilityData data) {
        Data = data;
        
        titleTmp.text = data.title;
        descriptionTmp.text = data.desc;
        costTmp.text = data.costAP.ToString();
        
        IsInteractable = true;
    }

    public override void Interact() { } //DO nothing, interact behaviour is just meant to reflect visuals

    protected override void OnBecameInteractable() {
        defaultStateFeel.PlayFeedbacks();
    }
    
    protected override void OnBecameUnInteractable() {
        oomStateFeel.PlayFeedbacks();
    }
}
