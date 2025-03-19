using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Player Stats")]
        [SerializeField] int level = 0;
        [SerializeField] int maxLevel = 1;
        [SerializeField] int XP = 0;
        [SerializeField] float maxHealth = 100;
        [SerializeField] float currentHealth;
        [SerializeField] float staminaStatModifier = 1.66f; 
        //[SerializeField] int mana = 100;
        [Header("References")]
        [SerializeField] PlayerStateMachine stateMachine;
        private void Awake()
        {
            stateMachine = GetComponent<PlayerStateMachine>();
        }
        private void Start()
        {
            maxHealth += Mathf.RoundToInt(stateMachine.CharacterLevel[CurrentLevel()].Stamina * staminaStatModifier);
            currentHealth = maxHealth;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (level == maxLevel)
                {
                    return;
                }
                XP++;
                if (XP >= stateMachine.CharacterLevel[CurrentLevel()].XpRequired)
                {
                    LevelUp();
                }
            }
        }
        
        public void PlayerTakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                stateMachine.ChangeState(new PlayerDeathState(stateMachine));
            }
        }
        public int CurrentLevel()
        {
            return this.level;
        }
        public void GainXP(int amount) 
        {
            if (level >= maxLevel)
                return;
            XP += amount;
            if (XP >= stateMachine.CharacterLevel[CurrentLevel()].XpRequired)
                LevelUp();
        }
        private void LevelUp()
        {
            XP = 0;
            level++;

            maxHealth += Mathf.RoundToInt(stateMachine.CharacterLevel[CurrentLevel()].Stamina * staminaStatModifier);
            currentHealth = maxHealth;

            Debug.Log($"Leveled up to {level}!");
        }
    }
}