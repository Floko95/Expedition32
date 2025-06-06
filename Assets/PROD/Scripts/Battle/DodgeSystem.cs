using System;
using ImprovedTimers;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private float parryWindowDuration;
    [SerializeField] private float dodgeWindowDuration;
    [SerializeField] private float jumpWindowDuration;
    
    [SerializeField] private InputActionReference parryInput;
    [SerializeField] private InputActionReference dodgeInput;
    [SerializeField] private InputActionReference jumpInput;
    
    [SerializeField] private Animator animator;
    [SerializeField] private MMF_Player onParryFeel;
    [SerializeField] private MMF_Player ondodgedFeel;
    
    [ShowInInspector] public bool IsDodging => _stateMachine is {currentState: Dodge};
    [ShowInInspector] public bool IsParrying => _stateMachine is {currentState: ParryState};
    [ShowInInspector] public bool IsJumping => _stateMachine is {currentState: JumpDodgeState};
    public DodgeStateMachine StateMachine => _stateMachine;
    
    private int _parryHash;
    private int _dodgeHash;
    private int _jumpHash;
    
    private DodgeStateMachine _stateMachine;
    private IdleDodgeState _idleDodgeState;
    
    private void Awake() {
        _parryHash = Animator.StringToHash("Parry");
        _dodgeHash = Animator.StringToHash("Dodge");
        _jumpHash = Animator.StringToHash("Jump");

        _stateMachine = new DodgeStateMachine();
        _idleDodgeState = new IdleDodgeState(_stateMachine);
        var parryState = new ParryState(_stateMachine, animator, _parryHash, parryWindowDuration);
        var dodge = new Dodge(_stateMachine, animator, _dodgeHash, dodgeWindowDuration);
        var jumpDodgeState = new JumpDodgeState(_stateMachine, animator, _jumpHash, jumpWindowDuration);
        
        _stateMachine.AddState(_idleDodgeState);
        _stateMachine.AddState(parryState);
        _stateMachine.AddState(dodge);
        _stateMachine.AddState(jumpDodgeState);
        
        _stateMachine.AddTransition(_idleDodgeState, parryState, parryInput.action.WasPerformedThisFrame);
        _stateMachine.AddTransition(_idleDodgeState, dodge, dodgeInput.action.WasPerformedThisFrame);
        _stateMachine.AddTransition(_idleDodgeState, jumpDodgeState, jumpInput.action.WasPerformedThisFrame);
    }

    private void OnEnable() {
        _stateMachine.SetState(_idleDodgeState);
    }

    private void OnDisable() {
        _stateMachine.SetState(_idleDodgeState);
    }

    private void Update() {
        _stateMachine.Update(Time.deltaTime);
    }
    
    public bool Evaluate(DodgeTypeEnum dodgeType) {
        if(enabled == false) return false;
        
        var doesNullifyDamage = dodgeType switch {
            DodgeTypeEnum.Undodgeable => false,
            DodgeTypeEnum.DodgeOrParry => IsDodging || IsParrying,
            DodgeTypeEnum.Jump => IsJumping,
            _ => throw new ArgumentOutOfRangeException(nameof(dodgeType), dodgeType, null)
        };
        
        //feedbacks
        if (doesNullifyDamage) {
            if (IsParrying) {
                onParryFeel?.PlayFeedbacks();
            } else if (IsDodging) {
                ondodgedFeel?.PlayFeedbacks();
            }
        }
        
        return doesNullifyDamage;
    }
}
