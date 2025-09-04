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

        float pullForce = 25f;
        int damage;
        float multiplier;

        [Header("Settings")]
        [SerializeField] LayerMask enemyLayer;
        [SerializeField] float spellMaxRadius = 5f;
        [SerializeField] float spellCenterRadius = 1f;
        [SerializeField] float damageCheckInterval = 0.1f;
        [SerializeField] float maxDuration = .3f;
        [SerializeField] float maxCastRange = 5f;

        float timer;
        float intervalTimer;

        private void OnEnable()
        {
            timer = maxDuration;
            intervalTimer = 0f;
            enemyList.Clear();

            Vector3 aimPoint = SkillManager.Instance.AimSpell();

            if (aimPoint == Vector3.zero)
            {
                Debug.Log("Invalid aim point.");
                gameObject.SetActive(false);
                return;
            }

            Vector3 playerPos = PlayerManager.Instance.PlayerStateMachine.transform.position;

            float distance = Vector3.Distance(playerPos, aimPoint);
            if (distance > maxCastRange)
            {
                Debug.Log("Target is too far away!");
                gameObject.SetActive(false);
                return;
            }

            transform.position = aimPoint;

            if (shouldShakeCamera)
                PlayerManager.Instance.PlayerStateMachine.CinemachineImpulseSource.GenerateImpulse();
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
            Collider[] hits = Physics.OverlapSphere(transform.position, spellMaxRadius, enemyLayer);

            foreach (Collider enemy in hits)
            {
                if (!enemy.TryGetComponent<ForceReceiver>(out var forceReceiver))
                    continue;

                if (!enemy.TryGetComponent<EnemyStateMachine>(out var enemySM))
                    continue;
                if(!enemySM.CanBeCrowdControlled) continue;
                if (enemySM.IsEnraged)
                    continue;

                Vector3 pullDir = (this.transform.position - enemy.transform.position).normalized;

                forceReceiver.AddForce(pullDir * pullForce);
                enemySM.ChangeState(new EnemyKnockbackState(enemySM, 0.5f));
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
            Gizmos.DrawWireSphere(transform.position, spellMaxRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, spellCenterRadius);
        }
#endif
    }
}
