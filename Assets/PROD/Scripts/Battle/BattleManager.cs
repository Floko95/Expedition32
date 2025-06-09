using System;
using System.Collections.Generic;
using BitDuc.Demo;
using BitDuc.EnhancedTimeline.Observable;
using R3;
using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    public static string ABILITY_CAM_NAME = "AbilityVCam";
    
    [SerializeField] private BattleData defaultData;
    [SerializeField] private List<Transform> enemySlots;
    [SerializeField] private List<Transform> alliesSlots;

    [SerializeField] private BehaviorGraphAgent behaviorGraphAgent;
    
    public UnityAction onBattleInitialized;
    
    public TurnQueue TurnQueue { get; set; }
    
    public Battle Battle { get; set; }
    
    public BattleData BattleData { get; set; }
    public bool isBattleInititated { get; set; }
    
    private TeamManager _teamManager;
    private CinemachineVirtualCameraBase _abilityvCam;
    private IDisposable _comboListener;
    private AbilityData _usedAbility;
    private List<Unit> _targets;
    private Unit _caster;
    
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


    public Observable<TimelineEvent> ExecuteAbility(Unit caster, List<Unit> targets, AbilityData usedAbility) {
        _caster = caster;
        _targets = targets;
        _usedAbility = usedAbility;
        
        if (usedAbility.timeline == null) {
            HandleComboWindow(null);
            return null;
        }
        
        _abilityvCam = caster.transform.Find(ABILITY_CAM_NAME)?.GetComponent<CinemachineVirtualCameraBase>();
        _abilityvCam.Priority = 100;
        
        
        var player = caster.playableDirector;
        
        _comboListener = player.Listen<ComboWindow>()
            .Subscribe(HandleComboWindow)
            .AddTo(caster.gameObject);

        var abilityObservable = player.Play(usedAbility.timeline);
        abilityObservable.Subscribe(
            onNext: _ => OnTimelineUpdate(),
            onCompleted: _ => OnTimelineComplete()
        );
        
        return abilityObservable;
    }
    
    private void OnTimelineUpdate() { }

    private void OnTimelineComplete() {
        _comboListener.Dispose();
        _abilityvCam.Priority = 0;
    }
    
    private void HandleComboWindow(ComboWindow window) {
        if(_usedAbility == null) return;
        
        foreach (var target in _targets) {
            if(target is AllyUnit allyUnit)
                BattleLogic.TryApplyAbilityEffects(_usedAbility, _caster, allyUnit);
            else
                _usedAbility.ApplyEffects(_caster, target);
        }
    }
}
