using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattleData defaultData;
    [SerializeField] private List<Transform> enemySlots;
    [SerializeField] private List<Transform> alliesSlots;

    [SerializeField] private BehaviorGraphAgent behaviorGraphAgent;
    
    public UnityAction onBattleInitialized;
    public UnityAction onBattleStart;
    public UnityAction<bool> onBattleEnd;

    public UnityAction<Unit> onTurnStart;
    public UnityAction onTurnPlay;
    public UnityAction onTurnEnd;
    
    public TurnQueue TurnQueue { get; set; }
    
    public Battle Battle { get; set; }
    public BattleData BattleData { get; set; }
    public bool isBattleInititated { get; set; }
    
    private TeamManager _teamManager;
    
    private async void Awake() {
        await Toolbox.WaitUntilReadyAsync();
        Toolbox.Set(this);
        behaviorGraphAgent.End();
    }

    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _teamManager = Toolbox.Get<TeamManager>();
        
        Initialize(BattleData ?? defaultData);
    }
    
    public virtual void Initialize(BattleData battleData) {
        BattleData = battleData;
        List<Unit> units = new List<Unit>();
        List<Unit> allies = new List<Unit>();
        List<Unit> enemies = new List<Unit>();
        
        //Create characters
        
        for (var index = 0; index < battleData.enemies.Count; index++) {
            var slot = enemySlots[index];
            var unit = Instantiate(battleData.enemies[index].prefab, slot).GetComponent<Unit>();
            units.Add(unit);
            enemies.Add(unit);
        }
        
        for (var index = 0; index < _teamManager.playableCharacters.Count; index++) {
            var slot = alliesSlots[index];
            var unit = Instantiate(_teamManager.playableCharacters[index].prefab, slot).GetComponent<Unit>();
            units.Add(unit);
            allies.Add(unit);
        }
        
        //Create battle
        Battle = new Battle {
            Units = units,
            Allies = allies,
            Enemies = enemies,
        };
        
        TurnQueue = new TurnQueue(Battle.Units);
        
        behaviorGraphAgent.Restart();
        
        onBattleInitialized?.Invoke();
        isBattleInititated = true;
    }

    public virtual void StartBattle() {
        Debug.Log("StartBattle");
        onBattleStart?.Invoke();
    }
    
    public virtual void EndTurn() {
        onTurnEnd?.Invoke();
    }
    
    public virtual void EndBattle(bool hasWon) {
        onBattleEnd?.Invoke(hasWon);
    }
}
