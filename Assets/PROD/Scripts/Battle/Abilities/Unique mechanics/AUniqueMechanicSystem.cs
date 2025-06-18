using System;
using UnityEngine;

public abstract class AUniqueMechanicSystem : MonoBehaviour, IInitializable<Unit>
{
    [SerializeField] public UniqueMechanicUI UIPrefab;
    
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract Sprite Icon { get; }

    public Action onMechanicUpdated;
    
    bool IInitializable<Unit>.Initialized { get; set; }
    protected Unit _unit;
    protected BattleManager _battleManager;

    protected virtual async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _battleManager = Toolbox.Get<BattleManager>();
        
        Init(GetComponent<Unit>());
    }
    
    public virtual void Init(Unit data) {
        if(data == null) return;
        _unit = data;
        
        BattleManager.onTurnStarted += OnTurnStarted;
        BattleLogic.DamagingEvent += OnDamageInflictedPostMitigation;
        BattleLogic.HealingEvent += OnUnitHealed;
        _unit.OnAbilityUsed += OnAbilityUsed;
    }
    
    private void OnDestroy() {
        BattleManager.onTurnStarted -= OnTurnStarted;
        BattleLogic.DamagingEvent -= OnDamageInflictedPostMitigation;
        BattleLogic.HealingEvent -= OnUnitHealed;
        _unit.OnAbilityUsed -= OnAbilityUsed;
    }

    //TODO set event abstract functions into a TurnBasedBehaviour
    protected virtual void OnTurnStarted(Unit unit) { }
    
    protected virtual void OnDamageInflictedPostMitigation(Unit source, Unit target, float amount, bool isCrit, ElementType damageType, ElementReaction elementalReaction) {}
    protected virtual void OnUnitHealed(Unit source, Unit target, float amount) { }
    protected virtual void OnUnitHealed() { }
    protected virtual void OnAbilityUsed(AbilityData ability) { }
}
