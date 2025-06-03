using System;
using ImprovedTimers;
using UnityEngine;
using Timer = ImprovedTimers.Timer;

public abstract class DodgeState : IState<DodgeState> {
    
    protected DodgeStateMachine _machine;

    public DodgeState(DodgeStateMachine machine) {
        _machine = machine;
    }

    public virtual void Enter(DodgeState previousState) { }
    
    public virtual void Update(float deltaTime) { }

    public virtual void FixedUpdate(float fixedDeltaTime) { }

    public virtual void Exit(DodgeState nextState) { }
}

public abstract class TimedDodgeState : DodgeState {
    float _duration;
    Timer _timer;
    Animator _animator;
    int _animationHash;
    
    protected TimedDodgeState(DodgeStateMachine machine, Animator animator, int animationHash, float duration) : base(machine) {
        _duration = duration;
        _animator = animator;
        _animationHash = animationHash;
    }

    public override void Enter(DodgeState previousState) {
        base.Enter(previousState);
        _timer = new CountdownTimer(_duration);
        _timer.OnTimerStop += OnTimerStop;
        _timer.Start();
        
        _animator.Play(_animationHash);
    }
    
    private void OnTimerStop() {
        _machine.SetState(_machine.GetState(typeof(IdleDodgeState)));
    }

    public override void Exit(DodgeState nextState) {
        base.Exit(nextState);
        _timer.Dispose();
    }
}

public class IdleDodgeState : DodgeState {
    public IdleDodgeState(DodgeStateMachine machine) : base(machine) { }
}


public class ParryState : TimedDodgeState {
    public ParryState(DodgeStateMachine machine, Animator animator, int animationHash, float duration) : base(machine, animator, animationHash, duration) { }
}

public class Dodge : TimedDodgeState {
    public Dodge(DodgeStateMachine machine, Animator animator, int animationHash, float duration) : base(machine, animator, animationHash, duration) { }
}

public class JumpDodgeState : TimedDodgeState {
    public JumpDodgeState(DodgeStateMachine machine, Animator animator, int animationHash, float duration) : base(machine, animator, animationHash, duration) { }
}


public class DodgeStateMachine : StateMachine<DodgeState> { }


public class DodgeSystem : MonoBehaviour
{
    [SerializeField] private float _type;
    [SerializeField] private Animator animator;
    
    public bool IsDodging => _stateMachine is {currentState: Dodge};
    public bool IsParrying => _stateMachine is {currentState: ParryState};
    public bool IsJumping => _stateMachine is {currentState: JumpDodgeState};
    
    private int _parryHash;
    private int _dodgeHash;
    private int _jumpHash;
    
    private DodgeStateMachine _stateMachine;

    private void Awake() {
        _parryHash = Animator.StringToHash("Parry");
        _dodgeHash = Animator.StringToHash("Dodge");
        _jumpHash = Animator.StringToHash("Jump");
        
        _stateMachine = new DodgeStateMachine();
        _stateMachine.AddState(new IdleDodgeState(_stateMachine));
        _stateMachine.AddState(new ParryState(_stateMachine, animator, _parryHash, _type));
        _stateMachine.AddState(new Dodge(_stateMachine, animator, _dodgeHash, _type));
        _stateMachine.AddState(new JumpDodgeState(_stateMachine, animator, _jumpHash, _type));
    }

    public bool Evaluate(DodgeTypeEnum dodgeType) {
        return dodgeType switch {
            DodgeTypeEnum.Undodgeable => false,
            DodgeTypeEnum.DodgeOrParry => IsDodging || IsParrying,
            DodgeTypeEnum.Jump => IsJumping,
            _ => throw new ArgumentOutOfRangeException(nameof(dodgeType), dodgeType, null)
        };
    }
}
