using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StateMachine<T> where T : IState<T> {

    public T currentState { get; private set; }
    public event Action<T> onStateChanged;
    
    private Dictionary<Type, T> _registeredStates = new();
    private Dictionary<Type, List<Transition<T>>> _transitions = new();
    private List<Transition<T>> _currentTransitions = new();
    private List<Transition<T>> _anyTransitions = new();

    public virtual void AddState(T state) {
        _registeredStates.Add(state.GetType(), state);
    }
    
    public virtual T GetState(Type stateType) {
        _registeredStates.TryGetValue(stateType, out T match);
        return match;
    }
    
    public virtual void SetState(T newState) {
        if (EqualityComparer<T>.Default.Equals(newState, currentState))
            return;

        currentState?.Exit(newState);
        Debug.Log($"Exiting {currentState}, entering {newState}");
        currentState = newState;

        _transitions.TryGetValue(currentState.GetType(), out _currentTransitions);
        _currentTransitions ??= new List<Transition<T>>();

        newState.Enter(newState);
        onStateChanged?.Invoke(newState);
    }

    public IEnumerator SetStateWithDelay(T newState, float delay) {
        Debug.Log("SetState with delay" + newState);
        yield return new WaitForSeconds(delay);
        SetState(newState);
    }
    
    public virtual void AddTransition(T from, T to, Func<bool> condition) {
        if (_transitions.TryGetValue(from.GetType(), out var transitions) == false) {
            transitions = new List<Transition<T>>();
            _transitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition<T>(to, condition));
    }

    public virtual void AddAnyTransition(T state, Func<bool> condition) {
        _anyTransitions.Add(new Transition<T>(state, condition));
    }

    public void Update(float deltaTime) {
        var transition = GetTransition();
        if (transition != null) {
            SetState(transition.to);
        }

        currentState.Update(deltaTime);
    }

    public void FixedUpdate(float fixedDeltaTime) {
        currentState.Update(fixedDeltaTime);
    }
    
    public Transition<T> GetTransition() {
        foreach (var transition in _anyTransitions.Where(transition => transition.condition())) {
            return transition;
        }

        return _currentTransitions.FirstOrDefault(transition => transition.condition());
    }
}

public class Transition<T> where T : IState<T> {

    public T to { get; }
    public Func<bool> condition { get; }

    public Transition(T to, Func<bool> condition) {
        this.to = to;
        this.condition = condition;
    }
}
   

