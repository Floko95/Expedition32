using System;
using System.Collections.Generic;
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
    public List<ITargetable> CurrentlyTargeted { get; private set; } = new();
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
        ClearAllTargets();
        SetTargetMode(AbilityTargetMode.SelectTarget);
    }
    
    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _battleManager = Toolbox.Get<BattleManager>();
        _battleManager.onBattleInitialized += OnBattleInitialized;

        _targetClockWiseInputaction = playerInput.actions.FindAction("TargetClockwise");
        _targetCounterClockWiseInputaction = playerInput.actions.FindAction("TargetCounterClockwise");
        
        _targetClockWiseInputaction.performed += OnTargetClockWiseInput;
        _targetCounterClockWiseInputaction.performed += OnTargetCounterClockWiseInput;
        
    }

    private void OnDestroy() {
        _battleManager.onBattleInitialized -= OnBattleInitialized;
        _targetClockWiseInputaction.performed -= OnTargetClockWiseInput;
        _targetCounterClockWiseInputaction.performed -= OnTargetCounterClockWiseInput;
    }

    private void OnBattleInitialized() {
        SetTargetMode(AbilityTargetMode.SelectTarget);
    }

    private void OnTargetCounterClockWiseInput(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;

        HandleCycleInput(false);
    }
    

    private void OnTargetClockWiseInput(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;

        HandleCycleInput(true);
    }

    private void SetTargetMode(AbilityTargetMode mode) {
        ClearAllTargets();
        _targetModeMode = mode;
        
        UpdateTargets();
    }

    private void UpdateTargets() {
        switch (_targetModeMode) {
            case AbilityTargetMode.AllEnemies:
                foreach (var enemy in _battleManager.Battle.AliveEnemies) AddTarget(enemy);
                break;
            case AbilityTargetMode.AllAllies:
                foreach (var ally in _battleManager.Battle.AliveAllies) AddTarget(ally);
                break;
            case AbilityTargetMode.SelectTarget:
                CycleTargets(_battleManager.Battle.AliveEnemies,false, true);
                break;
            case AbilityTargetMode.Ally:
                CycleTargets(_battleManager.Battle.AliveAllies, false, true);
                break;
            case AbilityTargetMode.CharacterSelf:
                ClearAllTargets();
                AddTarget(_battleManager.TurnQueue.CurrentTurn);
                onTargetChanged?.Invoke(_battleManager.TurnQueue.CurrentTurn);
                break;
            case AbilityTargetMode.DeadAllies:
                CycleTargets(_battleManager.Battle.DeadAllies, false, true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void AddTarget(ITargetable target) {
        if (target == null) return;
        if (CurrentlyTargeted.Contains(target)) return;
        
        target.OnTargeted();
        CurrentlyTargeted.Add(target);
    }
    
    private void ClearAllTargets() {
        foreach (var t in CurrentlyTargeted)
            t.OnUntargeted();
        CurrentlyTargeted.Clear();
    }
    
    private void CycleTargets(List<Unit> list, bool cycle, bool clockwise) {
        if (list.Count == 0) return;
        ClearAllTargets();

        if (cycle) {
            if (clockwise) {
                currentTargetIndex = (currentTargetIndex + 1) % list.Count;
            } else {
                currentTargetIndex = (currentTargetIndex - 1 + list.Count) % list.Count;
            }
        }
        
        AddTarget(list[currentTargetIndex]);
        
        onTargetChanged?.Invoke(list[currentTargetIndex]);
    }

    //TODO horrible code
    public void HandleCycleInput(bool clockwise) {
        if (_targetModeMode == AbilityTargetMode.SelectTarget)
            CycleTargets(_battleManager.Battle.AliveEnemies, true, clockwise);
        else if (_targetModeMode == AbilityTargetMode.Ally)
            CycleTargets(_battleManager.Battle.AliveAllies, true, clockwise);
    }
}
