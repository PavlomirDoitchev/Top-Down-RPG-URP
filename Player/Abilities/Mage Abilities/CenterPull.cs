using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Combat_Logic;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using Assets.Scripts.Player;

namespace Assets.Scripts.Player.Abilities.Mage_Abilities
{
    public class CenterPull : MonoBehaviour
    {
        [SerializeField] bool shouldShakeCamera = true;

        private readonly List<Collider> enemyList = new();

        float pullForce;
        int damage;
        float multiplier;

        [Header("Settings")]
        [SerializeField] LayerMask enemyLayer;
        [SerializeField] float maxRadius = 10f;
        [SerializeField] float impactRadius = 2f;
        [SerializeField] float damageCheckInterval = 0.1f;
        [SerializeField] float maxDuration = 1f;

        float timer;
        float intervalTimer;

        private void OnEnable()
        {
            timer = maxDuration;
            intervalTimer = 0f;
            enemyList.Clear();
            transform.position = SkillManager.Instance.AimSpell();

            if (shouldShakeCamera)
                PlayerManager.Instance.PlayerStateMachine.CinemachineImpulseSource.GenerateImpulse();

            // Get pull force from ability data
            pullForce = PlayerManager.Instance.PlayerStateMachine.Ability_Two_Data[
                PlayerManager.Instance.PlayerStateMachine.Ability_Two_Rank].knockbackForce;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                gameObject.SetActive(false);
                return;
            }

            intervalTimer -= Time.deltaTime;
            if (intervalTimer <= 0f)
            {
                PullEnemies();
                intervalTimer = damageCheckInterval;
            }
        }

        private void PullEnemies()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, maxRadius, enemyLayer);

            foreach (Collider enemy in hits)
            {
                if (!enemy.TryGetComponent<ForceReceiver>(out var forceReceiver))
                    continue;

                if (!enemy.TryGetComponent<EnemyStateMachine>(out var enemySM))
                    continue;

                if (enemySM.IsEnraged)
                    continue;

                Vector3 pullDir = (transform.position - enemy.transform.position).normalized;

                // Add pulling force towards the center
                forceReceiver.AddForce(pullDir * pullForce);

                // Optionally apply damage once per enemy
                if (!enemyList.Contains(enemy))
                {
                    ApplyDamage(enemy);
                    enemyList.Add(enemy);
                }
            }
        }

        private void ApplyDamage(Collider enemy)
        {
            if (enemy.gameObject.TryGetComponent<IDamagable>(out var damagable))
            {
                multiplier = PlayerManager.Instance.PlayerStateMachine.Ability_Two_Data[
                    PlayerManager.Instance.PlayerStateMachine.Ability_Two_Rank].damageMultiplier;

                damage = Mathf.RoundToInt(PlayerManager.Instance.PlayerStateMachine.WeaponDamage(damage, multiplier));
                damagable.TakeDamage(damage, false);

                PlayerManager.Instance.PlayerStateMachine.DamageText[3]
                    .Spawn(enemy.transform.position + Vector3.up * 2f, damage);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maxRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, impactRadius);
        }
#endif
    }
}
