using System;
using UnityEngine;

public abstract class GameState : IState<GameState> { 
    //insert common code for Game States here

    public StateMachine<GameState> StateMachine { get; private set; }
    
    public GameState(StateMachine<GameState> sm) {
        StateMachine = sm;
    }
    
    public abstract void Enter(GameState prevState);

    public abstract void FixedUpdate(float fixedDeltaTime);

    public abstract void Update(float deltaTime);
    public abstract void Exit(GameState nextState);
}

public class GameStateMachine  : StateMachine<GameState> {
    
}

[DefaultExecutionOrder(-10)]
public class GameStateManager : MonoBehaviour {
    
    public StateMachine<GameState> stateMachine { get; private set; }

    private void Awake() {
        Toolbox.Set(this);
        stateMachine = new GameStateMachine();
    }

    private void Start() {
        stateMachine.SetState(null);
    }
    
    private void Update() {
        stateMachine.Update(Time.deltaTime);
    }

    private void FixedUpdate() {
        stateMachine.FixedUpdate(Time.fixedDeltaTime);
    }
}
