using Assets.Scripts.State_Machine.Mount_State_Machine.States;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Mount_State_Machine
{
    public class MountStateMachine : StateMachine
    {
        [Header("References")]
        public Animator Animator { get; private set; }
        public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public MountInputManager InputManager { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [Header("Stats")]
        public float MaxSpeed = 10f;
        public float Acceleration = 5f;
        public float Deceleration = 6f;
        public float RotationSpeed = 5f;

        public float CurrentSpeed { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            CharacterController = GetComponent<CharacterController>();
            ForceReceiver = GetComponent<ForceReceiver>();
            InputManager = GetComponent<MountInputManager>();

        }
        private void Start()
        {
            ChangeState(new MountLocomotionState(this));
        }
        public void SetSpeed(float targetSpeed, float deltaTime)
        {
            CurrentSpeed = Mathf.MoveTowards(
                CurrentSpeed,
                targetSpeed,
                (targetSpeed > CurrentSpeed ? Acceleration : Deceleration) * deltaTime
            );

            Animator.SetFloat("MountedSpeed", CurrentSpeed / MaxSpeed, 0.1f, deltaTime);
        }
    }
}
