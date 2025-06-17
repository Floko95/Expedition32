using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UniqueMechanicsContainerUI : MonoBehaviour {

    [SerializeField] private RectTransform content;
    
    private BattleManager _battleManager;
    private Dictionary<Unit, UniqueMechanicUI> _UIs = new ();
    
    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _battleManager = Toolbox.Get<BattleManager>();

        if (_battleManager.isBattleInititated) {
            OnBattleInitialized();
        } else {
            _battleManager.onBattleInitialized += OnBattleInitialized;
        }
        
        BattleManager.onTurnStarted += OnTurnStarted;
        BattleManager.onTurnEnded += OnTurnEnded;
        
        //in case we missed turn start event
        OnTurnStarted(_battleManager.TurnQueue.CurrentTurn);
    }

    private void OnBattleInitialized() {
        foreach (var ally in _battleManager.Battle.Allies.Select(unit => unit as AllyUnit).Where(ally => ally.UniqueMechanicSystem != null)) {
            var ui = Instantiate(ally.UniqueMechanicSystem.UIPrefab, content);
            ui.Init(ally.UniqueMechanicSystem);
            
            _UIs.Add(ally, ui);
            ui.gameObject.SetActive(false);
            
        }
    }

    private void OnDestroy() {
        BattleManager.onTurnStarted -= OnTurnStarted;
        BattleManager.onTurnEnded -= OnTurnEnded;
    }
    
    private void OnTurnStarted(Unit unit) {
       if(_UIs.ContainsKey(unit) == false) return;
       
       _UIs[unit].gameObject.SetActive(true);
    }
    
    private void OnTurnEnded(Unit unit) {
        if(_UIs.ContainsKey(unit) == false) return;
        
        _UIs[unit].gameObject.SetActive(false);
    }
}
