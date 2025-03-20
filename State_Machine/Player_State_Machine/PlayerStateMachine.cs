using UnityEngine;
using Assets.Scripts.State_Machine;
using Assets.Scripts.State_Machine.Player;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerStateMachine : StateMachine
    {
        [Header("-----Equipped Items-----")]
        [SerializeField] public WeaponDataSO EquippedWeaponDataSO;
        [field: SerializeField] public GameObject EquippedWeapon { get; private set; }

        [Header("-----Character Levels-----")]
        [SerializeField] public CharacterLevelSO[] CharacterLevelDataSO;
        
        [Header("-----Ability Data-----")]
        [SerializeField] public AbilityDataSO[] AbilityDataSO;

        [Header("-----References-----")]
        [SerializeField] private GameObject rightHandEquipSlot;
        [SerializeField] private GameObject leftHandEquipSlot;
        [field: SerializeField] public InputManager InputManager { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        //[field: SerializeField] public PlayerStats PlayerStats { get; private set; }
        //[field: SerializeField] public Ragdoll Ragdoll { get; private set; }
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
            rb.linearVelocity = pushDir * CharacterLevelDataSO[PlayerStats.Instance.CurrentLevel()].CharacterPushObjectsForce;
        }
        public void EquipNewWeapon(WeaponDataSO newWeaponData, GameObject weaponPrefab)
        {
            if (EquippedWeaponDataSO != null)
            {
;                Destroy(EquippedWeapon);
            }
            //GameObject newWeapon = Instantiate(weaponPrefab, rightHandEquipSlot.transform.position,
            //    Quaternion.identity, rightHandEquipSlot.transform);
            GameObject newWeapon = Instantiate(weaponPrefab, Animator.GetBoneTransform(HumanBodyBones.RightIndexProximal));
            EquippedWeaponDataSO = newWeaponData;
            EquippedWeapon = newWeapon;
        }
    }
}