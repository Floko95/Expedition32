using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class StatusSystemUI : MonoBehaviour, IInitializable<Unit>
{
    [SerializeField] private RectTransform iconsContent;
    [SerializeField, AssetsOnly] private StatusUI statusUIPrefab;
    
    bool IInitializable<Unit>.Initialized { get; set; }
    
    private StatusSystem _statusSystem;
    private List<StatusUI> _statusUIList = new List<StatusUI>();
    
    private void Start() {
        Init(GetComponentInParent<Unit>());
    }

    public void Init(Unit unit) {
        if(unit == null) return;
        
        _statusSystem = unit.StatusSystem;
        _statusSystem.OnStatusChange += OnStatusChange;
        _statusSystem.OnStatusAdded += OnStatusAdded;
        _statusSystem.OnStatusRemoved += OnStatusRemoved;
    }

    private void OnStatusAdded(StatusInstance status) {
        var statusUI = Instantiate(statusUIPrefab, iconsContent);
        statusUI.Init(status);
        _statusUIList.Add(statusUI);
    }

    private void OnStatusChange(StatusInstance status) {
        var ui = _statusUIList.FirstOrDefault(ui => ui.Value == status);
        if(ui == null) return;
        ui.UpdateUI();
    }
    
    private void OnStatusRemoved(StatusInstance status) {
        var ui = _statusUIList.FirstOrDefault(ui => ui.Value == status);
        if(ui == null) return;
        
        Destroy(ui.gameObject);
    }
}
