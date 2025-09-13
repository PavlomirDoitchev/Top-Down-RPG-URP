using Assets.Scripts.State_Machine;
using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Mount_State_Machine
{
    public abstract class MountBaseState : State
    {
        protected readonly MountStateMachine stateMachine;

        protected MountBaseState(MountStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log($"[MOUNT] Entering state: {GetType().Name}");
        }

        /// <summary> Move the character controller with forward (local) motion plus vertical forces. </summary>
        protected void ApplyMovement(Vector3 localForwardDirection, float deltaTime)
        {
            // localForwardDirection = transform.forward * forwardInput * CurrentSpeed
            Vector3 horizontalMove = localForwardDirection * stateMachine.CurrentSpeed;
            Vector3 verticalMove = Vector3.zero;

            if (stateMachine.ForceReceiver != null)
            {
                verticalMove = stateMachine.ForceReceiver.Movement; // includes verticalVelocity
            }
            else
            {
                // fallback gravity if ForceReceiver missing
                verticalMove = Physics.gravity * deltaTime;
            }

            // CharacterController.Move expects meters per frame (delta)
            Vector3 moveThisFrame = (horizontalMove + verticalMove) * deltaTime;
            stateMachine.CharacterController.Move(moveThisFrame);
        }
        protected void RotateMountWithCamera(float deltaTime, float rotationMultiplier = 0.5f) 
        {
            Vector3 forward = stateMachine.MainCameraTransform.forward;
            Vector3 right = stateMachine.MainCameraTransform.right;
            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();
           stateMachine.transform.rotation = Quaternion.Slerp(
                stateMachine.transform.rotation,
                Quaternion.LookRotation(forward),
                stateMachine.RotationSpeed * rotationMultiplier * deltaTime
            );

        }
        protected void RotateMountToMouse(float deltaTime, float rotationMultiplier = 0.5f)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = LayerMask.GetMask("Ground", "Enemy", "Default", "Wall");

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                Vector3 targetPoint = hit.point;
                targetPoint.y = stateMachine.transform.position.y;
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - stateMachine.transform.position);

                stateMachine.transform.rotation = Quaternion.Slerp(
                    stateMachine.transform.rotation,
                    targetRotation,
                    stateMachine.RotationSpeed * rotationMultiplier * deltaTime
                );
            }
        }
    }
}
