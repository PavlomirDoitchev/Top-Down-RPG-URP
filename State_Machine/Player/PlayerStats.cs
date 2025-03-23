using Assets.Scripts.State_Machine.Player_State_Machine;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player
{
    public class PlayerStats : MonoBehaviour
    {
        public static PlayerStats Instance;
        [Header("Player Stats")]
        [SerializeField] int level = 0;
        [Tooltip("Set automatically in Start")]
        [SerializeField] int maxLevel = 1;
        [Tooltip("Required XP listed in LevelsSO")]
        [SerializeField] int currentXP = 0;
        [SerializeField] float maxHealth = 100;
        [SerializeField] float currentHealth;
        [SerializeField] float staminaStatModifier = 1.66f;

        [Header("Resource Info")]
        private CharacterLevelSO.ResourceType resourceType;
        [SerializeField] private int maxResource;
        [SerializeField] private int currentResource;
        bool canUseSkill = false;
        [Header("References")]
        [SerializeField] ParticleSystem levelUpEffect;
        public PlayerStateMachine playerStateMachine;

        [SerializeField] WeaponDataSO weaponDataSO;
        [SerializeField] GameObject weapon;

        private void Awake()
        {

            playerStateMachine = GetComponent<PlayerStateMachine>();
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            playerStateMachine.CharacterLevelDataSO[CurrentLevel()].ApplyClassModifiers();

            ApplyCharacterData();
            Debug.Log($"Loaded Class: {playerStateMachine.CharacterLevelDataSO[CurrentLevel()].GetCharacterClass()}");
            maxHealth += Mathf.RoundToInt(playerStateMachine.CharacterLevelDataSO[CurrentLevel()].Stamina * staminaStatModifier);
            currentHealth = maxHealth;
            maxLevel = playerStateMachine.CharacterLevelDataSO.Length - 1;
        }
        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (level == maxLevel)
                {
                    return;
                }
                currentXP++;
                if (currentXP >= playerStateMachine.CharacterLevelDataSO[CurrentLevel()].XpRequired)
                {
                    LevelUp();
                    playerStateMachine.EquipNewWeapon(weaponDataSO, weapon);
                }
            }
        }

        public void PlayerTakeDamage(int damage)
        {
            currentHealth -= damage;
            RegainResource(Mathf.RoundToInt(damage * 0.1f));
            if (currentHealth <= 0)
                playerStateMachine.ChangeState(new PlayerDeathState(playerStateMachine));
        }
        public int CurrentLevel()
        {
            return this.level;
        }
        public void GainXP(int amount)
        {
            if (level >= maxLevel)
                return;
            currentXP += amount;
            if (currentXP >= playerStateMachine.CharacterLevelDataSO[CurrentLevel()].XpRequired)
                LevelUp();
        }
        private void LevelUp()
        {
            currentXP = 0;
            level++;
            if (!levelUpEffect.isPlaying)
            {
                Instantiate(levelUpEffect, transform.position, Quaternion.identity);
            }

            maxHealth += Mathf.RoundToInt(playerStateMachine.CharacterLevelDataSO[CurrentLevel()].Stamina * staminaStatModifier);
            currentHealth = maxHealth;

            Debug.Log($"Leveled up to {level}!");
        }
        private void ApplyCharacterData()
        {
            resourceType = playerStateMachine.CharacterLevelDataSO[CurrentLevel()].GetResourceType();
            currentResource = maxResource;
            Debug.Log($"Player resource type set to: {resourceType}");
        }
        public bool CanUseSkill(int cost) 
        {
            if (currentResource >= cost) {
                UseResource(cost);
                Debug.Log($"Used {cost} {resourceType}. Remaining: {currentResource}");
                return canUseSkill = true;
            }
            else
                return canUseSkill = false;
        }
        public int UseResource(int amount)
        {
            return currentResource -= amount;
        }
        public void RegainResource(int amount)
        {
            currentResource = Mathf.Min(currentResource + amount, maxResource);
            Debug.Log($"Regained {amount} {resourceType}. Current: {currentResource}");
        }

        public CharacterLevelSO.ResourceType GetResourceType()
        {
            return resourceType;
        }
    }
}