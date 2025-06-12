using UnityEngine;
using Assets.Scripts.State_Machine;
using Assets.Scripts.Player;
using DamageNumbersPro;
using Unity.Cinemachine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    [RequireComponent(typeof(CharacterController), typeof(CinemachineImpulseSource), typeof(Rigidbody))]
    public class PlayerStateMachine : StateMachine
    {
        public State PlayerCurrentState => (State)CurrentState;
        [Header("-----Equipped Items-----")]
        [SerializeField] public WeaponDataSO EquippedWeaponDataSO;
        public GameObject EquippedWeapon;

        [Header("-----Character Levels-----")]
        public CharacterLevelSO[] CharacterLevelDataSO;

        [Header("-----Ability Data-----")]
        [Header("Basic Attack")]
        public Fighter_Ability_SO[] basicAbilityData;
        [field: SerializeField] public int BasicAttackRank { get; set; }

        [Tooltip("Value must be above 0 to be unlocked!")]
        [Header("Q Ability Ranks")]
        public Fighter_Ability_SO[] qAbilityData;
        [field: SerializeField] public int QAbilityRank { get; set; }   
        [Header("-----References-----")]
        [SerializeField] private GameObject rightHandEquipSlot;
        [SerializeField] private GameObject leftHandEquipSlot;
        [field: SerializeField] public InputManager InputManager { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

        public DamageNumber damageText;
        public PlayerStats PlayerStats { get; private set; }
        public Transform MainCameraTransform { get; private set; }
        [field: SerializeField] public CinemachineImpulseSource CinemachineImpulseSource { get; private set; }
        private void Start()
        {
            MainCameraTransform = Camera.main.transform;
            PlayerStats = GetComponent<PlayerStats>();
            if (PlayerStats.GetClassType() == CharacterLevelSO.CharacterClass.Fighter)
                ChangeState(new FighterLocomotionState(this));
            else if (PlayerStats.GetClassType() == CharacterLevelSO.CharacterClass.Mage)
                Debug.Log("where is the mage?!");
        }

        public void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb == null || rb.isKinematic)
                return;

            if (hit.moveDirection.y < -0.3f)
                return;

            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.linearVelocity = pushDir * PlayerStats.PushObjectsForce;
        }
        public void EquipNewWeapon(GameObject weaponPrefab)
        {
            if (EquippedWeapon != null)
            {
                Destroy(EquippedWeapon);
            }
            //GameObject newWeapon = Instantiate(weaponPrefab, rightHandEquipSlot.transform.position,
            //    Quaternion.identity, rightHandEquipSlot.transform);
            GameObject newWeapon = Instantiate(weaponPrefab, Animator.GetBoneTransform(HumanBodyBones.RightIndexProximal));
            EquippedWeapon = newWeapon;
        }
    }
}