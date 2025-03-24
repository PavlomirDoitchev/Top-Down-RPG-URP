using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerStats : MonoBehaviour
    {
        //public static PlayerStats Instance;
        [Header("Player Stats")]
        [SerializeField] int level = 0;
        [Tooltip("Set automatically in Start")]
        [SerializeField] int maxLevel = 1;
        [Tooltip("Required XP listed in LevelsSO")]
        [SerializeField] int currentXP = 0;
        [SerializeField] int maxHealth = 100;
        [SerializeField] int currentHealth;
        [SerializeField] float staminaStatModifier = 1.66f;
        //[SerializeField] float attackSpeed = 1f;
        //[SerializeField] float currentAttackSpeed = 1f;

        [Header("Resource Info")]
        private CharacterLevelSO.ResourceType resourceType;
        [SerializeField] private int maxResource;
        [SerializeField] private int currentResource;
        bool canUseSkill = false;

        [Header("References")]
        [SerializeField] ParticleSystem levelUpEffect;
        [SerializeField] WeaponDataSO weaponDataSO;
        [SerializeField] GameObject weapon;
        PlayerManager playerManager;

        private void Start()
        {
            playerManager = PlayerManager.Instance;
            playerManager.playerStateMachine.CharacterLevelDataSO[CurrentLevel()].ApplyClassModifiers();
            ApplyCharacterData();
            Debug.Log($"Loaded Class: {playerManager.playerStateMachine.CharacterLevelDataSO[CurrentLevel()].GetCharacterClass()}");
            maxHealth += Mathf.RoundToInt(playerManager.playerStateMachine.CharacterLevelDataSO[CurrentLevel()].Stamina * staminaStatModifier);
            currentHealth = maxHealth;
            maxLevel = playerManager.playerStateMachine.CharacterLevelDataSO.Length - 1;
        }
        //private void Update()
        //{

        //    if (Input.GetKeyDown(KeyCode.UpArrow))
        //    {
        //        if (level == maxLevel)
        //        {
        //            return;
        //        }
        //        currentXP++;
        //        if (currentXP >= playerStateMachine.CharacterLevelDataSO[CurrentLevel()].XpRequired)
        //        {
        //            LevelUp();
        //            playerStateMachine.EquipNewWeapon(weaponDataSO, weapon);
        //        }
        //    }
        //}

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
            currentXP = 0;
            level++;
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
    }
}