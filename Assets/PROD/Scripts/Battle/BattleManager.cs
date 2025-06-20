using System;
using System.Collections.Generic;
using System.Linq;
using BitDuc.Demo;
using BitDuc.EnhancedTimeline.Observable;
using DG.Tweening;
using R3;
using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BattleManager : MonoBehaviour
{
    public static string PLAYER_ABILITY_CAM_NAME = "PlayerAbilityVCam";
    public static string ENEMY_ABILITY_CAM_NAME = "EnemyAbilityVCam";
    
    [SerializeField] private BattleData defaultData;
    [SerializeField] private List<Transform> enemySlots;
    [SerializeField] private List<Transform> alliesSlots;
    [SerializeField] private InputActionReference QTEInput;
    [SerializeField] private QTEsUI qtEsUI;
    
    [SerializeField] private BehaviorGraphAgent behaviorGraphAgent;
    [SerializeField] private BattleStateEvent battleStateEvent;
    
    public UnityAction onBattleInitialized;
    public static UnityAction<Unit> onTurnStarted;
    public static UnityAction<Unit> onTurnEnded;
    
    public TurnQueue TurnQueue { get; set; }
    
    public Battle Battle { get; set; }
    
    public BattleData BattleData { get; set; }
    public bool isBattleInititated { get; set; }
    
    private TeamManager _teamManager;
    private CinemachineVirtualCameraBase _abilityvCam;
    private IDisposable _comboListener;
    private IDisposable _QTEListener;
    
    private AbilityData _usedAbility;
    private List<Unit> _targets;
    private Unit _caster;
    private bool _isCounterAvailable;
    
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
            if(battleData.enemies[index] == null) continue;
            
            var slot = enemySlots[index];
            var unit = Instantiate(battleData.enemies[index].prefab, slot).GetComponent<Unit>();
            units.Add(unit);
            enemies.Add(unit);
        }
        
        var usedTeam = battleData.forcedAllyTeam.Count > 0 ? battleData.forcedAllyTeam : _teamManager.playableCharacters;
        
        for (var index = 0; index < usedTeam.Count; index++) {
            if(usedTeam[index] == null) continue;
            
            var slot = alliesSlots[index];
            var unit = Instantiate(usedTeam[index].prefab, slot).GetComponent<Unit>();
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
        
        battleStateEvent.Event += OnBattleStateChanged;
    }

    private void OnBattleStateChanged(BattleState newstate) {
        if(newstate is not BattleState.Init) return;
        onTurnEnded.Invoke(TurnQueue.CurrentTurn);
    }


    public Observable<TimelineEvent> ExecuteAbility(Unit caster, List<Unit> targets, AbilityData usedAbility) {
        _caster = caster;
        _targets = targets;
        _usedAbility = usedAbility;
        _isCounterAvailable = true;
        
        if (usedAbility.timeline == null) {
            HandleComboWindow(null);
            return null;
        }
        
        //TODO horrible, move it in timeline animation priority propertties
        _abilityvCam = caster.transform.Find(PLAYER_ABILITY_CAM_NAME)?.GetComponent<CinemachineVirtualCameraBase>();
        if(_abilityvCam == null) _abilityvCam = caster.transform.Find(ENEMY_ABILITY_CAM_NAME)?.GetComponent<CinemachineVirtualCameraBase>();
        _abilityvCam.Priority = 100;
        
        if (targets.Count == 1 && usedAbility.targetMode is AbilityTargetMode.SelectTarget) {
            caster.transform.DOLookAt(targets.FirstOrDefault().transform.position, 0f, AxisConstraint.Y);
        }
        
        var player = caster.playableDirector;
        
        _comboListener = player.Listen<ComboWindow>()
            .Subscribe(HandleComboWindow)
            .AddTo(caster.gameObject);
        _QTEListener = player.Listen<QTE>().Subscribe(HandleQTE).AddTo(caster.gameObject);
            
        var abilityObservable = player.Play(usedAbility.timeline);
        abilityObservable.Subscribe(
            onNext: _ => OnTimelineUpdate(),
            onCompleted: _ => OnTimelineComplete()
        );
        
        return abilityObservable;
    }

    private void HandleQTE(QTE qte) {
        qte.isComplete = false;

        qtEsUI.StartQTE(qte);

        var clipStart = Time.time;
        var clipEnd = clipStart + (float)qte.clipDuration;
        
        qte.OnClipUpdate.Subscribe( frame => {
            var timeTillEnd = 1 - Mathf.InverseLerp(clipStart, clipEnd, Time.time);
            qtEsUI.UpdateQTECountdown(qte, timeTillEnd);

            if (QTEInput.action.WasPerformedThisFrame()) {
                qte.isComplete = true;
                qtEsUI.StopQTE(qte, true);
            }
        }, complete => {
            
            qtEsUI.StopQTE(qte, qte.isComplete);
        });
       
    }
    
    
    private void OnTimelineUpdate() { }

    private void OnTimelineComplete() {
        _comboListener.Dispose();
        _QTEListener.Dispose();
        
        _caster.transform.DOLocalRotate(Vector3.zero, 0.1f);
        _caster.transform.DOLocalMove(Vector3.zero, 0.25f);
        
        _abilityvCam.Priority = 0;
    }

    public bool CanCounter() => _isCounterAvailable;
    
    private void HandleComboWindow(ComboWindow window) {
        if(_usedAbility == null) return;
        
        foreach (var target in _targets) {
            if (target is AllyUnit allyUnit) {
                var effectApplication = BattleLogic.TryApplyAbilityEffects(_usedAbility, _caster, allyUnit);
                if (effectApplication.parried == false || effectApplication.negated == false) _isCounterAvailable = false;
            }
            else _usedAbility.ApplyEffects(_caster, target);
        }
    }
}
