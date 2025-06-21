using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UniqueMechanicUI : MonoBehaviour, IInitializable<AUniqueMechanicSystem> {
    
    [SerializeField] private TextMeshProUGUI titleTmp;
    [SerializeField] private TextMeshProUGUI descriptionTmp;
    [SerializeField] private Image icon;
    
    bool IInitializable<AUniqueMechanicSystem>.Initialized { get; set; }

    [SerializeField] private MMF_Player EnabledFeel;
    [SerializeField] private MMF_Player RefreshFeel;
    
    private AUniqueMechanicSystem _system;
    
    private void OnEnable() {
        EnabledFeel?.PlayFeedbacks();
    }
    
    public void Init(AUniqueMechanicSystem system) {
        _system = system;
        _system.onMechanicUpdated += OnMechanicUpdated;
        UpdateUI();
    }

    private void OnMechanicUpdated() {
        RefreshFeel?.PlayFeedbacks();
        UpdateUI();
    }

    private void OnDestroy() {
        _system.onMechanicUpdated -= UpdateUI;
    }
    
    public void UpdateUI() {
        titleTmp.text = _system.Title;
        titleTmp.color = _system.Color;
        descriptionTmp.text = _system.Description;
        icon.sprite = _system.Icon;
    }
}
