using UnityEngine;

namespace Assets.Scripts.State_Machine.Mount_State_Machine.States
{
    public class MountLocomotionState : MountBaseState
    {
        public MountLocomotionState(MountStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            base.EnterState();
            if (stateMachine.Animator != null)
                stateMachine.Animator.CrossFadeInFixedTime("Mount_Locomotion", 0.1f);
        }

        public override void UpdateState(float deltaTime)
        {
            Vector2 moveInput = stateMachine.InputManager != null ? stateMachine.InputManager.MovementInput() : Vector2.zero;
            float forwardInput = moveInput.y;

            float targetSpeed = forwardInput > 0.1f ? stateMachine.MaxSpeed : 0f;
            stateMachine.SetSpeed(targetSpeed, deltaTime);

            Vector3 forward = stateMachine.transform.forward * Mathf.Sign(Mathf.Max(0f, forwardInput));
            // Note: we use sign so backwards input doesn't make the horse run backward; adjust if you need reverse movement.

            ApplyMovement(forward, deltaTime);
            RotateMountWithCamera(deltaTime);
            //RotateMountToMouse(deltaTime);
        }

        public override void ExitState() { }
    }
}
