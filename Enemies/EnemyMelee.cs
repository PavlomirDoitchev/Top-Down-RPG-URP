using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyMelee : MonoBehaviour
    {
        public WeaponDataSO EquippedWeaponDataSO;
        private int baseDamage;
        private readonly List<Collider> enemyColliders = new List<Collider>();
        PlayerManager playerManager;
        private void Start()
        {
            playerManager = PlayerManager.Instance;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (enemyColliders.Contains(other)) return;
            if (other.gameObject.CompareTag("Player"))
            {
                enemyColliders.Add(other);

                var playerStats = playerManager.PlayerStateMachine.PlayerStats;
                playerStats.TakeDamage(baseDamage);
                Debug.Log("Player took " + baseDamage);

            }
        }
        public void EnemyWeaponDamage(int baseDamage, float multiplier)
        {
            this.baseDamage = Mathf.RoundToInt(baseDamage * multiplier);
        }
        public void EnemyClearHitEnemies()
        {
            enemyColliders.Clear();
        }
    }
}
