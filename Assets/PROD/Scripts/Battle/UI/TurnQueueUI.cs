using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class TurnQueueUI : MonoBehaviour {
    
    [SerializeField] private RectTransform contentRect;
    [SerializeField, AssetsOnly] private TurnUI turnUIPrefab;
    [SerializeField] private int maxTurnsDisplayed;
    
    private BattleManager _battleManager;
    private TurnQueue _turnQueue;

    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _battleManager = Toolbox.Get<BattleManager>();

        if (_battleManager.isBattleInititated) {
            OnBattleInitiated();
        }
        else {
            _battleManager.onBattleInitialized += OnBattleInitiated;
        }
    }

    private void OnBattleInitiated() {
        _turnQueue = _battleManager.TurnQueue;
        _turnQueue.onTurnsUpdated += OnTurnsUpdated;
        
        OnTurnsUpdated();
    }

    private void OnDestroy() {
        if(_turnQueue == null) return;
        
        _battleManager.onBattleInitialized -= OnBattleInitiated;
        _turnQueue.onTurnsUpdated -= OnTurnsUpdated;
    }

    private void OnTurnsUpdated() {
        foreach (Transform child in contentRect) {
            Destroy(child.gameObject);
        }
        
        int nbTurnsDisplayed = Mathf.Min(maxTurnsDisplayed, _turnQueue.turnQueue.Count);
        var turnsPeek = _turnQueue.PeekNextTurns(nbTurnsDisplayed);
        
        for (int i = 0; i < nbTurnsDisplayed; i++) {
            var turnUI = Instantiate(turnUIPrefab, contentRect.transform);
            turnUI.Init(turnsPeek[i].unitData);
        }
    }
}
