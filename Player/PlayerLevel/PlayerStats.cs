using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Player Level")]
        [SerializeField] int level = 0;
        [Tooltip("Set automatically in Start")]
        [SerializeField] int maxLevel = 1;
        [Tooltip("Required XP listed in LevelsSO")]
        [SerializeField] int currentXP = 0;

        [Header("Base Stats")]
        [field: SerializeField] public int Stamina { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }
        [field: SerializeField] public int Dexterity { get; private set; }
        [field: SerializeField] public int Intellect { get; private set; }
        [field: SerializeField] public int Wisdom { get; private set; }
        [field: SerializeField] public float BaseMovementSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float PushObjectsForce { get; private set; }
        [field: SerializeField] public float JumpForce { get; private set; }

        [Header("Secondary Stats")]
        [SerializeField] int maxHealth = 100;
        [SerializeField] int currentHealth;
        [SerializeField] float staminaStatModifier = 1.66f;
        [field: SerializeField] public float AttackSpeed { get; private set; }
        [field: SerializeField]
        [field:Range(0,1)]public float CriticalChance { get; private set; }
        [field: SerializeField]
        [field:Range(1, 5)] public float CriticalModifier { get; private set; }


        [Header("Resource Info")]
        private CharacterLevelSO.ResourceType resourceType;
        private CharacterLevelSO.CharacterClass characterClass;
        [SerializeField] private int maxResource;
        [SerializeField] private int currentResource;
        //bool canUseSkill = false;

        [Header("References")]
        [SerializeField] ParticleSystem levelUpEffect;
        [SerializeField] GameObject weapon;
        PlayerManager playerManager;
        private void Start()
        {
            playerManager = PlayerManager.Instance;
            playerManager.playerStateMachine.CharacterLevelDataSO[CurrentLevel()].ApplyClassResourceType();
            ApplyCharacterData();
            Debug.Log($"Loaded Class: {playerManager.playerStateMachine.CharacterLevelDataSO[CurrentLevel()].GetCharacterClass()}");
            maxHealth += Mathf.RoundToInt(playerManager.playerStateMachine.CharacterLevelDataSO[CurrentLevel()].Stamina * staminaStatModifier);
            currentHealth = maxHealth;
            SetMaxLevel();
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
                if (currentXP >= playerManager.playerStateMachine.CharacterLevelDataSO[CurrentLevel()].XpRequired)
                {
                    LevelUp();
                    playerManager.playerStateMachine.EquipNewWeapon(weapon);
                }
            }
        }

        public void PlayerTakeDamage(int damage)
        {
            currentHealth -= damage;
            Debug.Log($"Player has taken {damage}!");
            if (currentHealth <= 0)
                playerManager.playerStateMachine.ChangeState(new PlayerDeathState(playerManager.playerStateMachine));
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
            if (currentXP >= playerManager.playerStateMachine.CharacterLevelDataSO[CurrentLevel()].XpRequired)
                LevelUp();
        }
        private void LevelUp()
        {
            if (level >= maxLevel)
                return;
            level++;
            currentXP = 0;
            if (!levelUpEffect.isPlaying)
            {
                Instantiate(levelUpEffect, transform.position, Quaternion.identity);
            }

            maxHealth += Mathf.RoundToInt(playerManager.playerStateMachine.CharacterLevelDataSO[CurrentLevel()].Stamina * staminaStatModifier);
            currentHealth = maxHealth;

            Debug.Log($"Leveled up to {level}!");
        }
        private void ApplyCharacterData()
        {
            resourceType = playerManager.playerStateMachine.CharacterLevelDataSO[CurrentLevel()].GetResourceType();
            currentResource = maxResource;
            Debug.Log($"Player resource type set to: {resourceType}");
        }

        public int UseResource(int amount)
        {
            if (currentResource < 0)
                currentResource = 0;
            return currentResource -= amount;
        }
        public void RegainResource(int amount)
        {
            currentResource = Mathf.Min(currentResource + amount, maxResource);
            Debug.Log($"Regained {amount} {resourceType}. Current: {currentResource}");
        }
        public CharacterLevelSO.CharacterClass GetClassType() 
        {
            return characterClass;
        }
        public CharacterLevelSO.ResourceType GetResourceType()
        {
            return resourceType;
        }
        public int GetCurrentResource()
        {
            return currentResource;
        }
        public int GetCurrentHealth()
        {
            return currentHealth;
        }
        public int GetMaxHealth()
        {
            return maxHealth;
        }
        private void SetMaxLevel()
        {
            maxLevel = playerManager.playerStateMachine.CharacterLevelDataSO.Length - 1;
        }
    }
}