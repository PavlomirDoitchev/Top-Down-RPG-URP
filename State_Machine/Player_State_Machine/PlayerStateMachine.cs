using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using DamageNumbersPro;
using Unity.Cinemachine;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    [RequireComponent(typeof(CharacterController), typeof(CinemachineImpulseSource), typeof(Rigidbody))]
    public class PlayerStateMachine : StateMachine
    {
        public State PlayerCurrentState => (State)CurrentState;
        //[Header("-----Equipped Items-----")]
        //[SerializeField] public WeaponDataSO EquippedWeaponDataSO;
        [field: SerializeField] public GameObject EquippedWeapon { get; private set; } //remove later. Also remove from anim events!

        [Header("-----Character Levels-----")]
        public CharacterLevelSO[] CharacterLevelDataSO;

        [Header("-----Ability Data-----")]
        [field: SerializeField] public Fighter_Ability_SO[] BasicAttackData { get; private set; }
        [field: SerializeField] public int BasicAttackRank { get; set; }
        [field: SerializeField] public Fighter_Ability_SO[] Ability_One_Data { get; private set; }
        [field: SerializeField] public int Ability_One_Rank { get; set; }
        [field: SerializeField] public Fighter_Ability_SO[] Ability_Two_Data { get; private set; }
        [field: SerializeField] public int Ability_Two_Rank { get; set; }
        [field: SerializeField] public Fighter_Ability_SO[] Ability_Three_Data { get; private set; }
        [field: SerializeField] public int Ability_Three_Rank { get; set; }

        [field: SerializeField] public StatusEffectData[] EffectData { get; private set; }

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
            return statType switch
            {
                PlayerStatType.Strength => PlayerStats.Strength + PlayerStats.TotalStatChangeStrength,
                PlayerStatType.Dexterity => PlayerStats.Dexterity + PlayerStats.TotalStatChangeDexterity,
                PlayerStatType.Intellect => PlayerStats.Intellect + PlayerStats.TotalStatChangeIntellect,
                _ => (float)0,
            };
        }
        public enum EffectTypes
        {
            Poison,
            Burn,
            Slow,
            Bleed,
            Freeze,
            Stun
        }
        /// <summary>
        /// Applies a status effect to the target if it is effectable and the ability data indicates that a status effect should be applied.
        /// Index is used to determine which effect to apply from the EffectData array.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="abilityData"></param>
        /// <param name="spellRank"></param>
        /// <param name="index"></param>
        public void ApplyStatusEffect(Collider other, Fighter_Ability_SO[] abilityData, int spellRank, int index)
        {
            if (other.TryGetComponent<IEffectable>(out var effectable)
                && EffectData[index] != null
                && abilityData[spellRank].shouldApplyStatusEffect)
            {
                effectable.ApplyEffect(EffectData[index]);
            }
        }
        /// <summary>
        /// If ability has no data!
        /// </summary>
        /// <param name="other"></param>
        /// <param name="abilityData"></param>
        /// <param name="indexStatusEffect"></param>
        public void ApplyStatusEffect(Collider other, Fighter_Ability_SO abilityData, int indexStatusEffect)
        {
            if (other.TryGetComponent<IEffectable>(out var effectable)
                && EffectData[indexStatusEffect] != null
                && abilityData.shouldApplyStatusEffect)
            {
                effectable.ApplyEffect(EffectData[indexStatusEffect]);
            }
        }

    }
}