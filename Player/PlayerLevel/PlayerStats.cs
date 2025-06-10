using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Save_Manager;
using Assets.Scripts.Combat_Logic;

namespace Assets.Scripts.Player
{

    public class PlayerStats : MonoBehaviour, IDamagable, IEffectable, ISaveManager
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
        [SerializeField] int armor;
        [field: SerializeField] public float AttackSpeed { get; private set; }
        [field: SerializeField]
        [field: Range(0, 1)] public float CriticalChance { get; private set; }
        [field: SerializeField]
        [field: Range(1, 5)] public float CriticalModifier { get; private set; }


        [Header("Resource Info")]
        private CharacterLevelSO.ResourceType resourceType;
        private CharacterLevelSO.CharacterClass characterClass;
        [SerializeField] private int maxResource;
        [SerializeField] private int currentResource;

        [Header("References")]
        [SerializeField] ParticleSystem levelUpEffect;
        [SerializeField] GameObject weapon; //to be removed later
        //[SerializeField] private StatusEffectData effectData;
        PlayerManager playerManager;

        private class ActiveEffect
        {
            public StatusEffectData Data;
            public float ElapsedTime;
            public float NextTickTime;
            public int StackCount;
        }

        private Dictionary<StatusEffectData.StatusEffectType, ActiveEffect> activeEffects = new();
        private void Start()
        {
            playerManager = PlayerManager.Instance;
            playerManager.PlayerStateMachine.CharacterLevelDataSO[CurrentLevel()].ApplyClassResourceType();
            ApplyCharacterData();
            //Debug.Log($"Loaded Class: {playerManager.PlayerStateMachine.CharacterLevelDataSO[CurrentLevel()].GetCharacterClass()}");
            maxHealth *= (Stamina / 10);
            currentHealth = maxHealth;
            SetMaxLevel();
        }
        private void Update()
        {
            if (activeEffects != null)
            {
                HandleEffect();
            }
        }


        public void TakeDamage(int damage, bool applyImpulse = true)
        {
            currentHealth -= damage;

            if (applyImpulse)
            {
                playerManager.PlayerStateMachine.CinemachineImpulseSource.GenerateImpulse(Vector3.up * 0.1f);
                if (playerManager.PlayerStateMachine.PlayerCurrentState is FighterLocomotionState)
                {
                    playerManager.PlayerStateMachine.Animator.Play("Fighter_Hit");
                }
            }
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
            //Debug.Log($"Player resource type set to: {resourceType}");
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
                    Debug.Log($"Stacked {data.statusEffectType} to {existing.StackCount} stacks on player.");
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
        }

        public void RemoveEffect()
        {
            activeEffects = null;
            currentEffectTime = 0f;
            nextTickTime = 0f;
        }
        private float currentEffectTime = 0f;
        private float nextTickTime = 0f;
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
                    int totalDamage = effect.Data.DOTDamage * effect.StackCount;
                    TakeDamage(totalDamage, false);
                    effect.Data.DamageNumberPrefab.Spawn(transform.position, totalDamage);
                }
            }

            foreach (var key in toRemove)
            {
                activeEffects.Remove(key);
            }
        }
    }
    #endregion
}
