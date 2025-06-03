using System;
using System.Threading;
using System.Threading.Tasks;
using BitDuc.EnhancedTimeline.Animator;
using BitDuc.EnhancedTimeline.Timeline;
using R3;
using UnityEngine;
using UnityEngine.Playables;

namespace BitDuc.Demo
{
    public class Fighter : MonoBehaviour
    {
        enum State { Normal, FirstAttack, SecondAttack, Stagger }

        [SerializeField] PlayableAsset attack1;
        [SerializeField] PlayableAsset attack2;
        [SerializeField] PlayableAsset hit;
        [SerializeField] float walkSpeed = 1f;
        [SerializeField] float transitionSpeed = 0.01f;
        [SerializeField] float hitInvulnerabilityTime = 0.125f;
        [SerializeField] Collider weaponHitbox;

        TimelinePlayer player;
        AnimatorService animator;
        CharacterController controller;
        CancellationTokenSource attackCancel;
        FighterInput input;

        float walkAnimationSpeed;
        float movementSpeed;
        int comboCounter = 0;
        int invulnerable = 0;
        State state = State.Normal;

        void Awake()
        {
            enabled = false;
        }

        public void StartWith(TimelinePlayer player, AnimatorService animator, FighterInput input)
        {
            this.player = player;
            this.animator = animator;
            this.input = input;
            controller = GetComponent<CharacterController>();
            
            player.Listen<ComboWindow>()
                .Subscribe(HandleComboWindow)
                .AddTo(gameObject);

            player.Listen<MovementClip>()
                .Subscribe(HandleMovementClip)
                .AddTo(gameObject);

            enabled = true;
        }

        void Update()
        {
            var speed = movementSpeed;

            switch(state) {
                case State.Normal:
                    speed = UpdateIdle(speed);
                    break;
                case State.FirstAttack:
                    UpdateFirstAttack();
                    break;
                case State.Stagger:
                    break;
            }

            controller.Move(speed * Time.deltaTime * transform.forward);
            walkAnimationSpeed = Decay(walkAnimationSpeed, speed, transitionSpeed, Time.deltaTime);
            animator.SetFloat("Walk", walkAnimationSpeed);
        }

        void OnDestroy()
        {
            attackCancel?.Cancel();
        }

        float UpdateIdle(float speed)
        {
            var direction = Mathf.Sign(transform.forward.z);

            if (input.Left)
                speed += walkSpeed * -direction;

            if (input.Right)
                speed += walkSpeed * direction;

            if (input.Attack)
                Attack(attack1, State.FirstAttack);

            return speed;
        }

        void UpdateFirstAttack()
        {
            if (input.Attack && comboCounter > 0)
                Attack(attack2, State.SecondAttack);
        }

        static float Decay(float from, float to, float decay, float deltaTime) =>
            to + (from - to) * Mathf.Exp(-decay * deltaTime);

        void OnTriggerEnter(Collider other)
        {
            if (other == weaponHitbox || invulnerable > 0)
                return;

            Hit();
            TurnInvulnerable(hitInvulnerabilityTime);
        }
        
        async void Attack(PlayableAsset attackTimeline, State newState)
        {
            attackCancel?.Cancel();
            attackCancel = new CancellationTokenSource();
            state = newState;

            try
            {
                await player.Play(attackTimeline).WaitAsync(attackCancel.Token);
                state = State.Normal;
                movementSpeed = 0f;
            }
            catch (TaskCanceledException) { }
        }

        async void Hit()
        {
            attackCancel?.Cancel();
            state = State.Stagger;
            await player.Play(hit).WaitAsync();
            state = State.Normal;
            movementSpeed = 0f;
        }

        async void TurnInvulnerable(float seconds)
        {
            invulnerable++;
            await Observable.Timer(TimeSpan.FromSeconds(seconds)).WaitAsync();
            invulnerable--;
        }

        async void HandleComboWindow(ComboWindow comboWindow)
        {
            comboCounter++;
            await comboWindow.OnClipUpdate.WaitAsync();
            comboCounter--;
        }

        void HandleMovementClip(MovementClip movementClip)
        {
            movementClip.OnClipUpdate
                .Do(update => movementSpeed = update.behaviour.speed)
                .Subscribe();
        }
    }
}
