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
        public WeaponDataSO EquippedWeaponDataSO;
        private int baseDamage;
        private readonly List<Collider> enemyColliders = new List<Collider>();
        PlayerManager playerManager;
        EnemyStateMachine enemyStateMachine;
        public DamageNumber damageNumber;
        private void Start()
        {
            playerManager = PlayerManager.Instance;
            enemyStateMachine = GetComponentInParent<EnemyStateMachine>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (enemyColliders.Contains(other)) return;
            if (other.gameObject.CompareTag("Player") && this.gameObject.layer == LayerMask.NameToLayer("EnemyDamage"))
            {
                enemyColliders.Add(other);
                damageNumber.SetColor(Color.red);
                if (CriticalStrikeSuccessfull())
                {
                    baseDamage = Mathf.RoundToInt(Random.Range(EquippedWeaponDataSO.minDamage, EquippedWeaponDataSO.maxDamage + 1) * enemyStateMachine.CriticalModifier);
                    damageNumber.SetColor(Color.yellow);  
                    Debug.Log("Critical");
                }
                else
                    baseDamage = Random.Range(EquippedWeaponDataSO.minDamage, EquippedWeaponDataSO.maxDamage + 1);

                var playerStats = playerManager.PlayerStateMachine.PlayerStats;
                playerStats.TakeDamage(baseDamage);
                damageNumber.Spawn(other.transform.position, baseDamage, other.transform);
                //Debug.Log("Player took " + baseDamage);

            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (enemyColliders.Contains(other))
            {

                enemyColliders.Remove(other);
            }
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