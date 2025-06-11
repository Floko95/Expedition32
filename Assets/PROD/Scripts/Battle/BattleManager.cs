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
    public static string ABILITY_CAM_NAME = "AbilityVCam";
    
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
        
        if (usedAbility.timeline == null) {
            HandleComboWindow(null);
            return null;
        }
        
        _abilityvCam = caster.transform.Find(ABILITY_CAM_NAME)?.GetComponent<CinemachineVirtualCameraBase>();
        _abilityvCam.Priority = 100;
        
        if (targets.Count == 1) {
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
        
        if (_targets.Count == 1) {
            _caster.transform.DOLocalRotate(Vector3.zero, 0.1f);
        }
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
