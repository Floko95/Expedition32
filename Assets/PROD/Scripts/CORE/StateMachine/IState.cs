
public interface IState<in T> {
    
    void Enter(T previousState);
    void Update(float deltaTime);
    void FixedUpdate(float fixedDeltaTime);
    void Exit(T nextState);
}
