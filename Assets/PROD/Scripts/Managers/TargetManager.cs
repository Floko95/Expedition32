using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


public class TargetManager : MonoBehaviour {
    
    [SerializeField] private PlayerInput playerInput;
    
    public AbilityTarget TargetMode {
        get => _targetMode;
        set => SetTargetMode(value);
    }
    
    private BattleManager _battleManager;
    private InputAction _targetInputaction;
    
    private List<ITargetable> _allies;
    private List<ITargetable> _enemies;
    
    private AbilityTarget _targetMode = AbilityTarget.SelectTarget;
    private int currentTargetIndex = 0;
    private List<ITargetable> currentlyTargeted = new();
    
    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _battleManager = Toolbox.Get<BattleManager>();
        _battleManager.onBattleInitialized += OnBattleInitialized;

        _targetInputaction = playerInput.actions.FindAction("TargetClockwise");
        _targetInputaction.performed += OnTargetInput;
    }

    private void OnDestroy() {
        _battleManager.onBattleInitialized -= OnBattleInitialized;
        _targetInputaction.performed -= OnTargetInput;
    }

    private void OnBattleInitialized() {
        _enemies = _battleManager.Battle.Enemies.OfType<ITargetable>().ToList();
        _allies = _battleManager.Battle.Allies.OfType<ITargetable>().ToList();
    }

    private void OnTargetInput(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;

        HandleCycleInput();
    }

    private void SetTargetMode(AbilityTarget mode) {
        ClearAllTargets();
        _targetMode = mode;
        
        UpdateTargets();
    }

    private void UpdateTargets() {
        switch (_targetMode) {
            case AbilityTarget.AllEnemies:
                foreach (var enemy in _enemies) AddTarget(enemy);
                break;
            case AbilityTarget.AllAllies:
                foreach (var ally in _allies) AddTarget(ally);
                break;
            case AbilityTarget.SelectTarget:
                CycleTargets(_enemies);
                break;
            case AbilityTarget.Ally:
                CycleTargets(_allies);
                break;
        }
    }

    private void AddTarget(ITargetable target) {
        if (target == null) return;

        target.OnTargeted();
        currentlyTargeted.Add(target);
    }
    
    private void ClearAllTargets() {
        foreach (var t in currentlyTargeted)
            t.OnUntargeted();
        currentlyTargeted.Clear();
    }
    
    private void CycleTargets(List<ITargetable> list) {
        if (list.Count == 0) return;

        ClearAllTargets();
        
        currentTargetIndex = (currentTargetIndex + 1) % list.Count;
        AddTarget(list[currentTargetIndex]);
    }

    public void HandleCycleInput() {
        if (_targetMode == AbilityTarget.SelectTarget)
            CycleTargets(_enemies);
        else if (_targetMode == AbilityTarget.Ally)
            CycleTargets(_allies);
    }
}
