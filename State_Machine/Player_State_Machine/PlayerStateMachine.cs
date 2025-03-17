using UnityEngine;
using Assets.Scripts.State_Machine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerStateMachine : StateMachine
    {
        [Header("References")]
        [field: SerializeField] public InputManager InputManager { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public AttackData[] AttackData { get; private set; }
        [field: SerializeField] public float JumpForce = 5f;    
        public Transform MainCameraTransform { get; private set; }

        [Header("PlayerSettings")]
        [field: SerializeField] public float BaseMovementSpeed = 5f;
        [field: SerializeField] public float BaseRotationSpeed = 5f;
        [field: SerializeField] public float PushObjectsForce = 2f;    
        private void Start()
        {
            MainCameraTransform = Camera.main.transform;
            ChangeState(new PlayerLocomotionState(this));
        }
        public void OnControllerColliderHit(ControllerColliderHit hit)
        {
           Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb == null || rb.isKinematic)
                return;

            if (hit.moveDirection.y < -0.3f)
                return;

            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.linearVelocity = pushDir * PushObjectsForce;
        }
    }
}