using UnityEngine;
using Assets.Scripts.State_Machine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerStateMachine : StateMachine
    {
        [Header("References")]
        [field: SerializeField] public InputManager InputManager { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        public Transform MainCameraTransform { get; private set; }

        [Header("PlayerSettings")]
        [field: SerializeField] public float BaseMovementSpeed = 5f;
        [field: SerializeField] public float BaseRotationSpeed = 5f;
        [field: SerializeField] public float PushPower = 2f;    
        private void Start()
        {
            MainCameraTransform = Camera.main.transform;
            ChangeState(new TestState(this));
        }
        public void OnControllerColliderHit(ControllerColliderHit hit)
        {
           Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb == null || rb.isKinematic)
                return;

            if (hit.moveDirection.y < -0.3f)
                return;

            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.linearVelocity = pushDir * PushPower;
        }
    }
}