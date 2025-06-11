using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UnitHUD : MonoBehaviour, IInitializable<Unit>
{
    [SerializeField] private HealthBarUI healthBarUI;
    [SerializeField] private APBarUI apBarUI;
    [SerializeField] private Image iPortrait;
    
    [Title("Feedbacks")]
    [SerializeField] private MMF_Player turnStartFeel;
    [SerializeField] private MMF_Player turnEndedFeel;
    
    bool IInitializable<Unit>.Initialized { get; set; }
    
    private Unit _unit;
    private bool _myTurn;
    
    private void Awake() {
        Init(GetComponentInParent<Unit>());
    }

    public void Init(Unit unit) {
        if(unit == null) return;
        _unit = unit;
        
        healthBarUI.SetHealthSystem(unit.HealthSystem);
        if(apBarUI)
            apBarUI.Init(unit);
        
        if(iPortrait)
            iPortrait.sprite = unit.unitData.portrait;
        
        BattleManager.onTurnStarted += OnTurnStarted;
        BattleManager.onTurnEnded += OnTurnEnded;
    }

    private void OnTurnStarted(Unit unit) {
        if (unit != _unit || _myTurn) return;
        
        _myTurn = true;
        turnStartFeel?.PlayFeedbacks();
    }

    private void OnTurnEnded(Unit unit) {
        if (unit != _unit || !_myTurn) return;
        _myTurn = false;
        turnEndedFeel?.PlayFeedbacks();
    }
    
    private void OnDestroy() {
        BattleManager.onTurnStarted -= OnTurnStarted;
        BattleManager.onTurnEnded -= OnTurnEnded;
    }
}
