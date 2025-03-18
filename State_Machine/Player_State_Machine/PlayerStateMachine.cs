using UnityEngine;
using Assets.Scripts.State_Machine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerStateMachine : StateMachine
    {
        [Header("Stats")]
        [SerializeField] public CharacterStatsSO CharacterStats;
        [SerializeField] public WeaponDataSO EquippedWeapon;

        [Header("Attack Data")]
        [SerializeField] public AttackDataSO[] AttackData;
        [Header("References")]
        [field: SerializeField] public GameObject EquippedWeaponObject { get; private set; }
        [field: SerializeField] public InputManager InputManager { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }   
        public Transform MainCameraTransform { get; private set; } 
        
         
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
            rb.linearVelocity = pushDir * CharacterStats.PushObjectsForce;
        }

    }
}