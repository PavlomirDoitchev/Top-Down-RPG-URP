using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Save_Manager;
using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Utility.UI;
using Assets.Scripts.Player.Abilities.Fighter_Abilities;

namespace Assets.Scripts.Player
{

    public class PlayerStats : Subject, IDamagable, IEffectable, ISaveManager
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
        [SerializeField] int defense;
        [SerializeField] int resistance;
        [field: SerializeField] public float AttackSpeed { get; private set; }
        [field: SerializeField]
        [field: Range(0, 1)] public float CriticalChance { get; private set; }
        [field: SerializeField]
        [field: Range(1, 5)] public float CriticalDamageModifier { get; private set; }
        public bool ShouldKnockback { get; set; } = false;

        public float TotalSlowAmount => CalculateTotalSlow();
        public float TotalAttackSpeed => CalculateTotalAttackSpeed();
        public int TotalStatChange => CalculateTotalStatChange();
        public int TotalStatChangeStrength => CalculateTotalStatChange(StatusEffectData.ModifyMainStatType.Strength);
        public int TotalStatChangeDexterity => CalculateTotalStatChange(StatusEffectData.ModifyMainStatType.Dexterity);
        public int TotalStatChangeIntellect => CalculateTotalStatChange(StatusEffectData.ModifyMainStatType.Intellect);
        public int TotalStatChangeStamina => CalculateTotalStatChange(StatusEffectData.ModifyMainStatType.Stamina);

        [Header("Resource Info")]
        private CharacterLevelSO.ResourceType resourceType;
        private CharacterLevelSO.CharacterClass characterClass;
        [SerializeField] private int maxResource;
        [SerializeField] private int currentResource;

        [Header("References")]
        [SerializeField] ParticleSystem levelUpEffect;

        PlayerManager playerManager;
        float timer = 0;
        float timeSinceTakenDamage = 11;
        public class ActiveEffect
        {
            public StatusEffectData Data;
            public float ElapsedTime;
            public float NextTickTime;
            public int StackCount;
            public float CurrentSlowAmount => Data.SlowAmount * StackCount;
            public float CurrentAttackSpeed => Data.ModifyAttackSpeed * StackCount;
            public int CurrentStatChange => Data.ModifyMainStat * StackCount;
        }

        private Dictionary<StatusEffectData.StatusEffectType, ActiveEffect> activeEffects = new();
        private void Start()
        {
            playerManager = PlayerManager.Instance;
            playerManager.PlayerStateMachine.CharacterLevelDataSO[GetCurrentLevel()].ApplyClassResourceType();
            ApplyCharacterData();
            //Debug.Log($"Loaded Class: {playerManager.PlayerStateMachine.CharacterLevelDataSO[CurrentLevel()].GetCharacterClass()}");
            maxHealth *= (Stamina / 10);
            currentHealth = maxHealth;
            SetMaxLevel();
            NotifyObservers();
        }
        private void Update()
        {
            if (activeEffects != null && activeEffects.Count > 0)
                HandleEffect();

            timeSinceTakenDamage += Time.deltaTime;
            timer += Time.deltaTime;
            if (timer >= 1f && timeSinceTakenDamage <= 10)
            {
                RegainResource(1);
                timer = 0f;
            }
            else if (timeSinceTakenDamage > 10 && timer >= 1f)
            {
                Heal(Mathf.RoundToInt(maxHealth * 0.01f));
                UseResource(1);
                timer = 0f;
            }

        }

        public void TakeDOTDamage(int damage)
        {
            //damage -= resistance;
            if (damage <= 0) return;
            currentHealth -= damage;
            if (currentHealth <= 0)
                playerManager.PlayerStateMachine.ChangeState(new PlayerDeathState(playerManager.PlayerStateMachine));
            NotifyObservers();
        }
        public void TakeDamage(int damage, bool applyImpulse = true)
        {
            damage -= defense;
            if (damage <= 0) return;
            currentHealth -= damage;
            timeSinceTakenDamage = 0f;
            //if (damage >= Mathf.RoundToInt(maxHealth * .10f))
            //    playerManager.PlayerStateMachine.Animator.Play("Fighter_Hit");


            RegainResource(Mathf.RoundToInt((damage * 0.1f) / (GetCurrentLevel() + 1)));

            if (applyImpulse)
                playerManager.PlayerStateMachine.CinemachineImpulseSource.GenerateImpulse(Vector3.up * 0.1f);


            if (currentHealth <= 0)
                playerManager.PlayerStateMachine.ChangeState(new PlayerDeathState(playerManager.PlayerStateMachine));
            NotifyObservers();
        }
        public void Heal(int amount)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            if (currentHealth != maxHealth)
                playerManager.PlayerStateMachine.DamageText[5].Spawn(transform.position + Vector3.up * 2f, $"+{amount}+");
            NotifyObservers();
        }
        public Dictionary<StatusEffectData.StatusEffectType, ActiveEffect> GetActiveEffects()
        {
            return new Dictionary<StatusEffectData.StatusEffectType, ActiveEffect>(activeEffects);
        }
        public int GetCurrentLevel()
        {
            return this.level;
        }
        public int GetCurrentXP()
        {
            return this.currentXP;
        }   
        public int GetXPToNextLevel()
        {
            if (level >= maxLevel)
                return 0;
            return playerManager.PlayerStateMachine.CharacterLevelDataSO[GetCurrentLevel()].XpRequired;
        }
        public void GainXP(int amount)
        {
            if (level >= maxLevel)
                return;
            currentXP += amount;
            if (currentXP >= GetXPToNextLevel())
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
            resourceType = playerManager.PlayerStateMachine.CharacterLevelDataSO[GetCurrentLevel()].GetResourceType();
            currentResource = 0;
            //Debug.Log($"Player resource type set to: {resourceType}");
        }

        public void UseResource(int amount)
        {
            currentResource = Mathf.Max(currentResource - amount, 0);
            NotifyObservers();
        }
        public void RegainResource(int amount)
        {
            currentResource = Mathf.Min(currentResource + amount, maxResource);
            NotifyObservers();
            //Debug.Log($"Regained {amount} {resourceType}. Current: {currentResource}");
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
            return currentResource + playerManager.PlayerStateMachine.Weapon.resourceModifier;
        }
        public int GetMaxResource()
        {
            return maxResource + playerManager.PlayerStateMachine.Weapon.resourceModifier;
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
        #region Handle Status Effects
        public void ApplyEffect(StatusEffectData data)
        {
            if (activeEffects.TryGetValue(data.statusEffectType, out var existing))
            {
                if (data.IsStackable)
                {
                    existing.StackCount = Mathf.Min(existing.StackCount + 1, data.MaxStacks);
                    existing.ElapsedTime = 0f;
                    existing.NextTickTime = 0f;

                    if (data.StackRefreshDuration > 0)
                        existing.ElapsedTime = 0;
                    //Debug.Log($"Stacked {data.statusEffectType} to {existing.StackCount} stacks on player.");
                }
                else
                {
                    existing.ElapsedTime = 0f;
                    existing.NextTickTime = 0f;
                }

            }
            else
            {
                activeEffects[data.statusEffectType] = new ActiveEffect
                {
                    Data = data,
                    StackCount = 1,
                    ElapsedTime = 0f,
                    NextTickTime = 0f
                };
            }
            NotifyObservers();
        }

        public void HandleEffect()
        {

            List<StatusEffectData.StatusEffectType> toRemove = new();

            foreach (var kvp in activeEffects)
            {

                var effect = kvp.Value;
                effect.ElapsedTime += Time.deltaTime;

                if (effect.ElapsedTime >= effect.Data.DOTDuration)
                {
                    toRemove.Add(kvp.Key);
                    continue;
                }
                if (effect.Data.DOTDamage != 0 && effect.ElapsedTime > effect.NextTickTime)
                {

                    effect.NextTickTime += effect.Data.DOTInterval;
                    int totalDamage = (effect.Data.DOTDamage * effect.StackCount) - resistance;
                    TakeDOTDamage(totalDamage);
                    if (totalDamage > 0)
                        effect.Data.DamageNumberPrefab.Spawn(transform.position, totalDamage);

                }
                NotifyObservers();
            }

            foreach (var key in toRemove)
            {
                activeEffects.Remove(key);
                NotifyObservers();
            }
        }

        private float CalculateTotalSlow()
        {
            float maxSlow = 0f;
            foreach (var effect in activeEffects.Values)
            {
                if (effect.Data.SlowAmount > 0)
                {
                    maxSlow = Mathf.Max(maxSlow, effect.CurrentSlowAmount);

                }
            }
            return maxSlow;
        }
        private float CalculateTotalAttackSpeed()
        {
            float totalAttackSpeed = AttackSpeed + playerManager.PlayerStateMachine.Weapon.attackSpeedModifier;
            foreach (var effect in activeEffects.Values)
            {
                if (effect.Data.ModifyAttackSpeed != 0)
                {
                    totalAttackSpeed *= (1 + effect.Data.ModifyAttackSpeed);
                }
            }
            return totalAttackSpeed;
        }
        /// <summary>
        /// Target specific stat to change.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private int CalculateTotalStatChange(StatusEffectData.ModifyMainStatType status)
        {
            int totalStatChange = 0 + playerManager.PlayerStateMachine.Weapon.strengthModifier;
            foreach (var effect in activeEffects.Values)
            {
                if (effect.Data.ModifyMainStat != 0)
                {
                    if (effect.Data.modifyStat == status)
                        totalStatChange += effect.Data.ModifyMainStat * effect.StackCount;
                }
            }
            return totalStatChange;
        }
        /// <summary>
        /// Affects all stats.
        /// </summary>
        /// <returns></returns>
        private int CalculateTotalStatChange()
        {
            int totalStatChange = 0;
            foreach (var effect in activeEffects.Values)
            {
                if (effect.Data.ModifyMainStat != 0)
                {
                    totalStatChange += effect.Data.ModifyMainStat * effect.StackCount; //affect all stats?

                }
            }
            return totalStatChange;
        }
    }

    #endregion
}
