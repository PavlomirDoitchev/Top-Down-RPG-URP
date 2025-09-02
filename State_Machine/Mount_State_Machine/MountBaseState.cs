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
        /// <summary>
        /// Preserve momentum. Used in MountMove().
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="deltaTime"></param>
        protected void Move(Vector3 movement, float deltaTime) 
        {
            stateMachine.CharacterController.Move(movement * deltaTime);
        }
        /// <summary>
        /// Move horse forward in its local forward direction
        /// </summary>
        /// <param name="deltaTime"></param>
        protected void MountMove(float deltaTime)
        {
            Vector3 forward = CalculateMovement();
            Move(forward, deltaTime);

            if (forward.magnitude > 0f)
            {
                stateMachine.Animator.SetFloat("MountedSpeed", stateMachine.CurrentSpeed, 0.1f, deltaTime);
            }
            else
            {
                stateMachine.Animator.SetFloat("MountedSpeed", 0f, 0.1f, deltaTime);
            }
        }
        private Vector3 CalculateMovement() 
        {
            Vector2 moveInput = stateMachine.InputManager.MovementInput();
            Vector3 forward = stateMachine.transform.forward * moveInput.y;
            return forward.normalized * stateMachine.CurrentSpeed;
        }
        /// <summary>
        /// Rotate mount towards the mouse position with heavier/slow turning.
        /// </summary>
        protected void RotateMountToMouse(float deltaTime, float rotationMultiplier = 0.5f)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = LayerMask.GetMask("Ground", "Enemy");

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                Vector3 targetPoint = hit.point;
                targetPoint.y = stateMachine.transform.position.y;
                Quaternion targetRotation = Quaternion.LookRotation(
                    targetPoint - stateMachine.transform.position);

                stateMachine.transform.rotation = Quaternion.Slerp(
                    stateMachine.transform.rotation,
                targetRotation,
                        stateMachine.RotationSpeed * rotationMultiplier * deltaTime
                );
            }
        }
    }
}
