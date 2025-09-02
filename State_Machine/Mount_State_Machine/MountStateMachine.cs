using UnityEngine;

namespace Assets.Scripts.State_Machine.Mount_State_Machine
{
    public class MountStateMachine : StateMachine
    {
        [Header("References")]
        public Animator Animator { get; private set; }
        public CharacterController Controller { get; private set; }

        [Header("Stats")]
        public float MaxSpeed = 10f;
        public float Acceleration = 5f;
        public float Deceleration = 6f;
        public float RotationSpeed = 5f;

        public float CurrentSpeed { get; private set; }

        private void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            Controller = GetComponent<CharacterController>();
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
