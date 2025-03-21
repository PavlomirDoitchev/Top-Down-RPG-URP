using Assets.Scripts.State_Machine.Player_State_Machine;
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
        [SerializeField] private int currentResource;
        private int maxResource;

        [Header("References")]
        [SerializeField] PlayerStateMachine stateMachine;
        [SerializeField] ParticleSystem levelUpEffect;

        [SerializeField] WeaponDataSO weaponDataSO;
        [SerializeField] GameObject weapon;

        private void Awake()
        {
            
            stateMachine = GetComponent<PlayerStateMachine>();
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
            stateMachine.CharacterLevelDataSO[CurrentLevel()].ApplyClassModifiers();
            ApplyCharacterData();
            Debug.Log($"Loaded Class: {stateMachine.CharacterLevelDataSO[CurrentLevel()].GetCharacterClass()}");
            maxHealth += Mathf.RoundToInt(stateMachine.CharacterLevelDataSO[CurrentLevel()].Stamina * staminaStatModifier);
            currentHealth = maxHealth;
            maxLevel = stateMachine.CharacterLevelDataSO.Length - 1;
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
                if (currentXP >= stateMachine.CharacterLevelDataSO[CurrentLevel()].XpRequired)
                {
                    LevelUp();
                    stateMachine.EquipNewWeapon(weaponDataSO, weapon);
                }
            }
        }

        public void PlayerTakeDamage(int damage)
        {
            currentHealth -= damage;
            RegainResource(Mathf.RoundToInt(damage * 0.1f));
            if (currentHealth <= 0)
                stateMachine.ChangeState(new PlayerDeathState(stateMachine));
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
            if (currentXP >= stateMachine.CharacterLevelDataSO[CurrentLevel()].XpRequired)
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

            maxHealth += Mathf.RoundToInt(stateMachine.CharacterLevelDataSO[CurrentLevel()].Stamina * staminaStatModifier);
            currentHealth = maxHealth;

            Debug.Log($"Leveled up to {level}!");
        }
        private void ApplyCharacterData() 
        {
            resourceType = stateMachine.CharacterLevelDataSO[CurrentLevel()].GetResourceType();
            maxResource = stateMachine.CharacterLevelDataSO[CurrentLevel()].maxResource;
            currentResource = stateMachine.CharacterLevelDataSO[CurrentLevel()].maxResource;
            Debug.Log($"Player resource type set to: {resourceType}, Max Resource: {maxResource}");
        }
        public void UseResource(int amount)
        {
            if (currentResource >= amount)
            {
                currentResource -= amount;
                Debug.Log($"Used {amount} {resourceType}. Remaining: {currentResource}");
            }
            else
            {
                Debug.LogWarning("Not enough resource!");
            }
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