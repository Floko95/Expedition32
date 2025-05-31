using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class APBarUI : MonoBehaviour, IInitializable<Unit>
{
    [SerializeField] private MMProgressBar progressBar;
    [SerializeField] private TextMeshProUGUI amountTmp;
    
    bool IInitializable<Unit>.Initialized { get; set; }
    
    private APSystem _apSystem;

    private void Start() {
        Init(GetComponentInParent<Unit>());
    }

    public void Init(Unit data) {
        if(data == null) return;
        
        _apSystem = data.APSystem;
        _apSystem.OnAPChanged += ONAPChanged;
        
        ONAPChanged();
    }

    private void ONAPChanged() {
        progressBar.SetBar(_apSystem.AP, 0, _apSystem.MaxAP);
        amountTmp.text = _apSystem.AP.ToString();
    }
}
