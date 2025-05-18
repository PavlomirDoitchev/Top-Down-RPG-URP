using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Save_Manager;

namespace Assets.Scripts.Player
{
    public class PlayerStats : MonoBehaviour, ISaveManager
    {
        public void LoadData(GameData _data)
        {
            this.level = _data.playerLevel;
            currentXP = _data.playerExperience;
            //transform.position = _data.GetPlayerPosition();
            //transform.rotation = _data.GetPlayerRotation();
            //currentHealth = _data.currentHealth;
            //currentResource = _data.currentResource;
        }

        public void SaveData(ref GameData _data)
        {
            _data.playerLevel = this.level;
            _data.playerExperience = currentXP;
            //_data.SavePlayerTransform(transform.position, transform.rotation);
            //_data.currentHealth = currentHealth;
            //_data.currentResource = currentResource;
        }

        // Buff and Debuff system (commented out for now)
    
        //public event Action<Buff> OnBuffApplied;
        //public event Action<Buff> OnBuffExpired;

        //public event Action<Debuff> OnDebuffApplied;
        //public event Action<Debuff> OnDebuffExpired;

        //private List<Buff> activeBuffs = new List<Buff>();
        //private List<Debuff> activeDebuffs = new List<Debuff>();

       

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
        [SerializeField] int armor;
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

        [Header("References")]
        [SerializeField] ParticleSystem levelUpEffect;
        [SerializeField] GameObject weapon;
        PlayerManager playerManager;
        private void Start()
        {
            playerManager = PlayerManager.Instance;
            playerManager.PlayerStateMachine.CharacterLevelDataSO[CurrentLevel()].ApplyClassResourceType();
            ApplyCharacterData();
            Debug.Log($"Loaded Class: {playerManager.PlayerStateMachine.CharacterLevelDataSO[CurrentLevel()].GetCharacterClass()}");
            maxHealth *= (Stamina / 10);
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
                if (currentXP >= playerManager.PlayerStateMachine.CharacterLevelDataSO[CurrentLevel()].XpRequired)
                {
                    LevelUp();
                    playerManager.PlayerStateMachine.EquipNewWeapon(weapon);
                }
            }
        }

        public void PlayerTakeDamage(int damage)
        {
            currentHealth -= damage;
            Debug.Log($"Player has taken {damage}!");
            if (currentHealth <= 0)
                playerManager.PlayerStateMachine.ChangeState(new PlayerDeathState(playerManager.PlayerStateMachine));
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
            if (currentXP >= playerManager.PlayerStateMachine.CharacterLevelDataSO[CurrentLevel()].XpRequired)
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
            
            currentHealth = maxHealth;

            Debug.Log($"Leveled up to {level}!");
        }
        private void ApplyCharacterData()
        {
            resourceType = playerManager.PlayerStateMachine.CharacterLevelDataSO[CurrentLevel()].GetResourceType();
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
            maxLevel = playerManager.PlayerStateMachine.CharacterLevelDataSO.Length - 1;
        }
        //public void ApplyBuff(Buff buff)
        //{
        //    activeBuffs.Add(buff);
        //    OnBuffApplied?.Invoke(buff); // Notify listeners
        //    StartCoroutine(HandleBuff(buff));
        //}

        //public void ApplyDebuff(Debuff debuff)
        //{
        //    activeDebuffs.Add(debuff);
        //    OnDebuffApplied?.Invoke(debuff); // Notify listeners
        //    StartCoroutine(HandleDebuff(debuff));
        //}
        //private IEnumerator HandleBuff(Buff buff)
        //{
        //    ApplyBuffEffect(buff, true);
        //    yield return new WaitForSeconds(buff.Duration);
        //    ApplyBuffEffect(buff, false);
        //    activeBuffs.Remove(buff);
        //    OnBuffExpired?.Invoke(buff);
        //}

        //private IEnumerator HandleDebuff(Debuff debuff)
        //{
        //    ApplyDebuffEffect(debuff, true);
        //    yield return new WaitForSeconds(debuff.Duration);
        //    ApplyDebuffEffect(debuff, false);
        //    activeDebuffs.Remove(debuff);
        //    OnDebuffExpired?.Invoke(debuff);
        //}

        //private void ApplyBuffEffect(Buff buff, bool isApplying)
        //{
        //    float modifier = isApplying ? buff.EffectStrength : -buff.EffectStrength;

        //    switch (buff.Type)
        //    {
        //        case BuffType.AttackBoost:
        //            ModifyAttack(modifier);
        //            break;
        //        case BuffType.SpeedBoost:
        //            ModifySpeed(modifier);
        //            break;
        //        case BuffType.DefenseBoost:
        //            ModifyDefense(modifier);
        //            break;
        //    }
        //}

        //private void ApplyDebuffEffect(Debuff debuff, bool isApplying)
        //{
        //    float modifier = isApplying ? debuff.EffectStrength : -debuff.EffectStrength;

        //    switch (debuff.Type)
        //    {
        //        case DebuffType.Slow:
        //            ModifySpeed(modifier);
        //            break;
        //        case DebuffType.Weaken:
        //            ModifyAttack(modifier);
        //            break;
        //        case DebuffType.Stun:
        //            ApplyStun(isApplying);
        //            break;
        //    }
        //}

        //private void ModifyAttack(float amount) => Debug.Log($"Attack modified by {amount}");
        //private void ModifySpeed(float amount) => Debug.Log($"Speed modified by {amount}");
        //private void ModifyDefense(float amount) => Debug.Log($"Defense modified by {amount}");
        //private void ApplyStun(bool isApplying) => Debug.Log(isApplying ? "Player Stunned!" : "Player Recovered!");
    }
}
