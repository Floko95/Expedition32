using Unity.Cinemachine;
using UnityEngine;

public class BattleCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineStateDrivenCamera allyTurnCam;

    private BattleManager _battleManager;
    
    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _battleManager = Toolbox.Get<BattleManager>();
        
        if (_battleManager.isBattleInititated) {
            OnBattleInitiated();
        } else {
            _battleManager.onBattleInitialized += OnBattleInitiated;
        }
    }

    private void OnBattleInitiated() {
        _battleManager.onTurnStart += OnTurnStart;
        _battleManager.onTurnEnd += OnTurnEnd;
    }

    private void OnTurnStart(Unit unit) {
        if(unit.isEnemy) return;
        var allyUnit = unit as AllyUnit;
        
        allyTurnCam.Follow = unit.transform;
        allyTurnCam.AnimatedTarget = allyUnit.animator;
        
        allyTurnCam.Follow.LookAt(unit.transform);
        allyTurnCam.Priority = 11;
    }
    
    private void OnTurnEnd() {
        
    }
}
