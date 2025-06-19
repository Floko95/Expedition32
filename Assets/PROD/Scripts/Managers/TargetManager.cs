using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetManager : MonoBehaviour {
    
    [SerializeField] private PlayerInput playerInput;
    
    public AbilityTargetMode TargetMode {
        get => _targetModeMode;
        set => SetTargetMode(value);
    }
    
    public Action<ITargetable> onTargetChanged;
    [ShowInInspector] public List<ITargetable> CurrentlyTargeted { get; private set; } = new();
    [ShowInInspector] public List<ITargetable> AvailableTargets { get; private set; } = new();
    [ShowInInspector] public int TargetCount => CurrentlyTargeted?.Count ?? 0;
    
    private BattleManager _battleManager;
    private InputAction _targetClockWiseInputaction;
    private InputAction _targetCounterClockWiseInputaction;
    
    private AbilityTargetMode _targetModeMode = AbilityTargetMode.SelectTarget;
    private int currentTargetIndex = 0;

    private async void Awake() {
        await Toolbox.WaitUntilReadyAsync();
        Toolbox.Set(this);
    }

    public void Reset() {
        SetTargetMode(AbilityTargetMode.SelectTarget);
    }
    
    private void OnEnable() {
        _targetClockWiseInputaction ??= playerInput.actions.FindAction("TargetClockwise");
        _targetCounterClockWiseInputaction ??= playerInput.actions.FindAction("TargetCounterClockwise");
        
        _targetClockWiseInputaction.performed += OnTargetClockWiseInput;
        _targetCounterClockWiseInputaction.performed += OnTargetCounterClockWiseInput;
    }

    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _battleManager = Toolbox.Get<BattleManager>();
        _battleManager.onBattleInitialized += OnBattleInitialized;
    }
    
    private void OnDisable() {
        _targetClockWiseInputaction.performed -= OnTargetClockWiseInput;
        _targetCounterClockWiseInputaction.performed -= OnTargetCounterClockWiseInput;
    }

    private void OnDestroy() {
        _battleManager.onBattleInitialized -= OnBattleInitialized;
    }

    private void OnBattleInitialized() {
        SetTargetMode(AbilityTargetMode.SelectTarget);
    }

    private void SetTargetMode(AbilityTargetMode mode) {
        ClearAllTargets();
        _targetModeMode = mode;
        
        switch (_targetModeMode) {
            case AbilityTargetMode.AllEnemies:
                AvailableTargets = new List<ITargetable>(_battleManager.Battle.AliveEnemies);
                foreach (var enemy in AvailableTargets) AddTarget(enemy);
                break;
            case AbilityTargetMode.AllAllies:
                AvailableTargets = new List<ITargetable>(_battleManager.Battle.AliveAllies);
                foreach (var ally in _battleManager.Battle.AliveAllies) AddTarget(ally);
                break;
            case AbilityTargetMode.SelectTarget:
                AvailableTargets = new List<ITargetable>(_battleManager.Battle.AliveEnemies);
                AddTarget(_battleManager.Battle.AliveEnemies[currentTargetIndex]);
                break;
            case AbilityTargetMode.Ally:
                AvailableTargets = new List<ITargetable>(_battleManager.Battle.AliveAllies);
                AddTarget(_battleManager.Battle.AliveAllies[currentTargetIndex]);
                break;
            case AbilityTargetMode.CharacterSelf:
                AvailableTargets = new List<ITargetable> { _battleManager.TurnQueue.CurrentTurn };
                AddTarget(_battleManager.TurnQueue.CurrentTurn);
                break;
            case AbilityTargetMode.DeadAllies:
                AvailableTargets = new List<ITargetable> ( _battleManager.Battle.DeadAllies );
                AddTarget(_battleManager.Battle.DeadAllies.FirstOrDefault());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    private void AddTarget(ITargetable target) {
        target.OnTargeted();
        CurrentlyTargeted.Add(target);
        
        onTargetChanged?.Invoke(target);
    }
    
    private void ClearAllTargets() {
        foreach (var t in CurrentlyTargeted)
            t.OnUntargeted();
        CurrentlyTargeted.Clear();
    }

    private void CycleTargets(List<ITargetable> list, bool clockwise) {
        if (list.Count == 0) return;
        ClearAllTargets();

        if (clockwise) {
            currentTargetIndex = (currentTargetIndex + 1) % list.Count;
        }
        else {
            currentTargetIndex = (currentTargetIndex - 1 + list.Count) % list.Count;
        }

        AddTarget(list[currentTargetIndex]);
    }

    //TODO horrible code
    
    private void OnTargetCounterClockWiseInput(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        HandleCycleInput(false);
    }
    

    private void OnTargetClockWiseInput(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        HandleCycleInput(true);
    }

    public void HandleCycleInput(bool clockwise) {
        if(_targetModeMode is AbilityTargetMode.AllAllies or AbilityTargetMode.AllEnemies) return;
        
        CycleTargets(AvailableTargets, clockwise);
    }
}
