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
        //[Header("-----Equipped Items-----")]
        //[SerializeField] public WeaponDataSO EquippedWeaponDataSO;
        //public GameObject EquippedWeapon;

        [Header("-----Character Levels-----")]
        public CharacterLevelSO[] CharacterLevelDataSO;

        [Header("-----Ability Data-----")]
        [field: SerializeField] public Fighter_Ability_SO[] BasicAttackData { get; private set; }
		[field: SerializeField] public int BasicAttackRank { get; set; }
        [field: SerializeField] public Fighter_Ability_SO[] Ability_One_Data { get; private set; }
        [field: SerializeField] public int Ability_One_Rank { get; set; }
        [field: SerializeField] public Fighter_Ability_SO[] Ability_Two_Data { get; private set; }
		[field: SerializeField] public int Ability_Two_Rank { get; set; }

        #region Equipment
        [field: SerializeField] public WeaponDataSO Weapon { get; set; }
		#endregion

		#region Global Flags
		//public bool ShouldKnockback { get; set; }
		
		#endregion
		[Space(20)]
		[Header("-----References-----")]
        [SerializeField] private GameObject rightHandEquipSlot;
        [SerializeField] private GameObject leftHandEquipSlot;
        [field: SerializeField] public InputManager InputManager { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public AnimationNamesData AnimationNamesData { get; private set; }
		[field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

        [field: SerializeField] public DamageNumber[] DamageText { get; private set; }
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
		public bool IsInState<T>() where T : State
		{
			return CurrentState is T;
		}
        public bool CriticalStrikeSuccess()
		{
			return Random.Range(0f, 1f) <= PlayerStats.CriticalChance + Weapon.criticalChanceModifier;
		}
        public int WeaponDamage(int damage, float abilityMultiplier) 
        {
            damage = Mathf.RoundToInt(Random.Range(Weapon.minDamage, Weapon.maxDamage + 1));
            if (Weapon.weaponType == WeaponDataSO.WeaponType.Warhammer 
				|| Weapon.weaponType == WeaponDataSO.WeaponType.TwoHandedSword
				|| Weapon.weaponType == WeaponDataSO.WeaponType.TwoHandedAxe)
			{
				damage += Mathf.RoundToInt(GetStatValue(PlayerStatType.Strength) * abilityMultiplier);
			}
			else if (Weapon.weaponType == WeaponDataSO.WeaponType.Staff)
			{
				damage += Mathf.RoundToInt(GetStatValue(PlayerStatType.Intellect) * abilityMultiplier);
			}
			//Add more weapons if needed
			return damage;
		}
		public enum PlayerStatType
		{
			Strength,
			Dexterity,
			Intellect
		}
		public float GetStatValue(PlayerStatType statType)
		{
			switch (statType)
			{
				case PlayerStatType.Strength:
					return PlayerStats.Strength + PlayerStats.TotalStatChangeStrength;
				case PlayerStatType.Dexterity:
					return PlayerStats.Dexterity + PlayerStats.TotalStatChangeDexterity;
				case PlayerStatType.Intellect:
					return PlayerStats.Intellect + PlayerStats.TotalStatChangeIntellect;
				default:
					return 0;
			}
		}
		
       
    }
}