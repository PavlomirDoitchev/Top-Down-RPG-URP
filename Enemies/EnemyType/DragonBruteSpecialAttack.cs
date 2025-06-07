using Assets.Scripts.State_Machine.Player_State_Machine;
using DamageNumbersPro;
using UnityEngine;
namespace Assets.Scripts.Enemies.EnemyType
{
    public class DragonBruteSpecialAttack : MonoBehaviour
    {

        [SerializeField] ParticleSystem fireBreathEffect;
        [SerializeField] DamageNumber damageNumberPrefab;
        [Header("AttackStats")]
        [SerializeField] private int damage = 25;
        [SerializeField] private float radius = 5f;
        [SerializeField] LayerMask playerLayer;
        private void Update()
        {

            if(Input.GetKeyDown(KeyCode.F))
            {
                if (!fireBreathEffect.isPlaying)
                    fireBreathEffect.Play();
                Collider[] hits = Physics.OverlapSphere(transform.position, radius, playerLayer);
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent<PlayerStateMachine>(out var player))
                    {
                        player.PlayerStats.TakeDamage(damage);
                    }
                }
            }

        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(transform.position, radius);
            
        }
    }
}
