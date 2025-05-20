using UnityEngine;

public abstract class ABattleAllyState : IState<ABattleAllyState> {

    public ABattleAllyState (AllyUnit allyUnit) {
        
    }
        
    public abstract void Enter(ABattleAllyState previousState);
    
    public abstract void Update(float deltaTime);

    public abstract void FixedUpdate(float fixedDeltaTime);

    public abstract void Exit(ABattleAllyState nextState);
}
