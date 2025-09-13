using UnityEngine;

namespace Assets.Scripts.State_Machine.Mount_State_Machine
{
    [RequireComponent(typeof(ForceReceiver), typeof(MountInputManager), typeof(CharacterController))]
    public class MountStateMachine : StateMachine
    {
        [Header("References")]
        public Animator Animator { get; private set; }
        public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public MountInputManager InputManager { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        public Transform MainCameraTransform { get; private set; }

        [Header("Stats")]
        public float MaxSpeed = 6f;
        public float Acceleration = 8f;
        public float Deceleration = 10f;
        public float RotationSpeed = 5f;

        public float CurrentSpeed { get; private set; }

        private void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            CharacterController = GetComponent<CharacterController>();
            ForceReceiver = GetComponent<ForceReceiver>();
            InputManager = GetComponent<MountInputManager>();
            MainCameraTransform = Camera.main.transform;
        }

        private void Start()
        {
            ChangeState(new States.MountLocomotionState(this));
        }

        public void SetSpeed(float targetSpeed, float deltaTime)
        {
            CurrentSpeed = Mathf.MoveTowards(
                CurrentSpeed,
                Mathf.Clamp(targetSpeed, 0f, MaxSpeed),
                (targetSpeed > CurrentSpeed ? Acceleration : Deceleration) * deltaTime
            );
            if (Animator != null)
            {
                Animator.SetFloat("MountedSpeed", CurrentSpeed / MaxSpeed, 0.1f, deltaTime);
            }
        }
    }
}
