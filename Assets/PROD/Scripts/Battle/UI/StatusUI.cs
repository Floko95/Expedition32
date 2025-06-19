using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour, IInitializable<StatusInstance>
{
    [SerializeField] private Image statusIcon;
    [SerializeField] private TextMeshProUGUI stackCountTmp;

    bool IInitializable<StatusInstance>.Initialized { get; set; }
    
    public StatusInstance Value { get; set; }
    
    public void Init(StatusInstance status) {
        Value = status;
        
        statusIcon.sprite = status.data.icon;
    }

    public void UpdateUI() {
        stackCountTmp.gameObject.SetActive(Value.data.isStackable);
        stackCountTmp.text = Value.StackCount.ToString();
    }
}
