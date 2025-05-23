using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BattleHUD : MonoBehaviour {
    
    [SerializeField, SceneObjectsOnly] private List<UnitHUD> unitHUDs;
    
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
        for (int i = 0; i < _battleManager.Battle.Allies.Count; i++) {
            if(i > unitHUDs.Count) break;
            
            unitHUDs[i].Init(_battleManager.Battle.Allies[i]);
        }
    }
}
