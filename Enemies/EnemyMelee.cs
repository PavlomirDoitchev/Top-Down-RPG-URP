using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
namespace Assets.Scripts.Enemies
{
    public class EnemyMelee : MonoBehaviour
    {
        private readonly List<Collider> enemyColliders = new List<Collider>();
        
        [Header("References")]
        public WeaponDataSO EquippedWeaponDataSO;
        [SerializeField] private StatusEffectData effectData;
        EnemyStateMachine enemyStateMachine;
        PlayerManager playerManager;
        public DamageNumber damageNumber;
        [SerializeField] float damageTextOffsetY = 2f; // Offset for the damage text above the enemy
        [SerializeField] ParticleSystem hitParticleSystem;

        private int baseDamage;
        Vector3 damageTextPos;

        private void Start()
        {
            playerManager = PlayerManager.Instance;
            enemyStateMachine = GetComponentInParent<EnemyStateMachine>();
        }
        private void OnTriggerStay(Collider other)
        {
            if (enemyColliders.Contains(other)) return;
            if (other.gameObject.layer == LayerMask.NameToLayer("MyOutlines") 
                && other.TryGetComponent<IDamagable>(out var damagable)
                && this.gameObject.layer == LayerMask.NameToLayer("EnemyDamage"))
            {
                if (effectData != null && other.TryGetComponent<IEffectable>(out var effectable)
                && other.gameObject.layer == LayerMask.NameToLayer("MyOutlines"))
                {
                    effectable.ApplyEffect(effectData);
                }
                enemyColliders.Add(other);

                if (hitParticleSystem != null)
                {
                    hitParticleSystem.transform.position = other.transform.position;
                    if (hitParticleSystem.isPlaying)
                    {
                        hitParticleSystem.Stop();
                        hitParticleSystem.Clear();
                        hitParticleSystem.Emit(1);
                    }
                    hitParticleSystem.Emit(1); // Emit a single particle at the enemy's position
                }
                damageNumber.SetColor(Color.red);
                
                if (CriticalStrikeSuccessfull())
                {
                    baseDamage = Mathf.RoundToInt(Random.Range(EquippedWeaponDataSO.minDamage, EquippedWeaponDataSO.maxDamage + 1) * enemyStateMachine.CriticalModifier);
                    damageNumber.SetColor(Color.yellow);  
                }
                else
                    baseDamage = Random.Range(EquippedWeaponDataSO.minDamage, EquippedWeaponDataSO.maxDamage + 1);

                if(enemyStateMachine.IsEnraged)
                {
                    baseDamage = Mathf.RoundToInt(baseDamage * enemyStateMachine.EnragedDamageMultiplier);
                }
                if (enemyStateMachine.ShouldKnockBackPlayer)
                    playerManager.PlayerStateMachine.ForceReceiver
                        .AddForce((other.transform.position - enemyStateMachine.transform.position).normalized * enemyStateMachine.KnockBackForce);

                var playerStats = playerManager.PlayerStateMachine.PlayerStats;
                playerStats.TakeDamage(baseDamage, false);

                damageTextPos = other.transform.position + Vector3.up * damageTextOffsetY; // Adjust the position to be above the enemy
                damageNumber.Spawn(damageTextPos, baseDamage, other.transform);
                //Debug.Log("Player took " + baseDamage);

            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (enemyColliders.Contains(other))
                enemyColliders.Remove(other);
        }
        private bool CriticalStrikeSuccessfull()
        {
            float criticalChance = enemyStateMachine.CriticalChance;
            return Random.Range(0f, 1f) <= criticalChance;
        }
        public void EnemyWeaponDamage(int baseDamage, float multiplier)
        {
            this.baseDamage = Mathf.RoundToInt(baseDamage * multiplier);
        }

        public void SetEnemyLayerDuringAttack()
        {
            this.gameObject.layer = LayerMask.NameToLayer("EnemyDamage");
        }
        public void CantHarmPlayer()
        {
            this.gameObject.layer = LayerMask.NameToLayer("CantHarmPlayer");
        }
    }
}