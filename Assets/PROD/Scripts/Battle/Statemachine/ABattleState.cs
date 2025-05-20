using UnityEngine;

public abstract class ABattleState : IState<ABattleState>
{

    protected BattleManager _battleManager;

    public ABattleState(BattleManager battleManager) {
        _battleManager = battleManager;
    }

    public abstract void Enter(ABattleState previousState);

    public abstract void Update(float deltaTime);

    public abstract void FixedUpdate(float fixedDeltaTime);

    public abstract void Exit(ABattleState nextState);
}
