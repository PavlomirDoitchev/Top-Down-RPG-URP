using Assets.Scripts.State_Machine.Player_State_Machine;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Player Stats")]
        [SerializeField] int level = 0;
        [Tooltip("Set automatically in Start")]
        [SerializeField] int maxLevel = 1;
        [Tooltip("Required XP listed in LevelsSO")]
        [SerializeField] int currentXP = 0;
        [SerializeField] float maxHealth = 100;
        [SerializeField] float currentHealth;
        [SerializeField] float staminaStatModifier = 1.66f;
        //[SerializeField] int mana = 100;

        [Header("References")]
        [SerializeField] PlayerStateMachine stateMachine;
        [SerializeField] ParticleSystem levelUpEffect;

        [SerializeField] WeaponDataSO weaponDataSO;
        [SerializeField] GameObject weapon;

        private void Awake()
        {
            stateMachine = GetComponent<PlayerStateMachine>();
        }
        private void Start()
        {
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
            if (stateMachine.CharacterLevelDataSO[CurrentLevel()].Class == "Fighter")
            { 
                //Add Rage
            }
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
    }
}