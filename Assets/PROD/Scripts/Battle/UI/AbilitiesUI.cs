using System.Collections.Generic;
using UnityEngine;

public class AbilitiesUI : MonoBehaviour, IInitializable<UnitData>
{
    [SerializeField] private List<AbilityUI> abilityUIs;
    
    bool IInitializable<UnitData>.Initialized { get; set; }

    private BattleManager _battleManager;
    private Unit _myUnit;
    
    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _battleManager = Toolbox.Get<BattleManager>();

        _myUnit = GetComponentInParent<Unit>();
        if (_battleManager.isBattleInititated) {
            OnBattleInitialized();
        }
        else {
            _battleManager.onBattleInitialized += OnBattleInitialized;
        }
    }

    private void OnDestroy() {
        _battleManager.onBattleInitialized -= OnBattleInitialized;
    }

    private void OnBattleInitialized() {
        Init(_myUnit.unitData);
    }

    public void Init(UnitData data) {
        
        for (int i = 0; i < abilityUIs.Count; i++) {
            
            if (i < data.abilities.Count) {
                abilityUIs[i].Init(data.abilities[i]);
                abilityUIs[i].gameObject.SetActive(true);
            }
            else {
                abilityUIs[i].gameObject.SetActive(false);
            }
        }
    }
}
