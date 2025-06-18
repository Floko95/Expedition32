using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour {
    
    //TODO swap for Character specific UI
    [SerializeField] private UnitHUD unitHUDPrefab;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    
    private BattleManager _battleManager;

    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _battleManager = Toolbox.Get<BattleManager>();

        if (_battleManager.isBattleInititated) {
            OnBattleInitialized();
        } else {
            _battleManager.onBattleInitialized += OnBattleInitialized;
        }
    }

    private void OnBattleInitialized() {
        foreach (var ally in _battleManager.Battle.Allies) {
            var ui = Instantiate(unitHUDPrefab, layoutGroup.transform);
            ui.Init(ally);
        }

        //stupid
        layoutGroup.enabled = true;
        this.InvokeInOneFrame(() => layoutGroup.enabled = false);
    }
}
