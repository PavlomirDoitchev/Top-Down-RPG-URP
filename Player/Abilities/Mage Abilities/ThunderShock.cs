using Assets.Scripts.Combat_Logic;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player.Abilities.Mage_Abilities
{
    public class ThunderShock : MonoBehaviour
    {
        [SerializeField] bool shouldShakeCamera = true;

        private readonly List<Collider> enemyList = new();

        float pushForce = 10f;
        int damage;
        float multiplier;
        float timer;
        float vfxTimer;
        float intervalTimer;
        [Header("Settings")]
        [SerializeField] LayerMask enemyLayer;
        [SerializeField] float spellMaxRadius = 5f;
        [SerializeField] float spellCenterRadius = 1f;
        [SerializeField] float vfxDuration = 2f;

        private void OnEnable()
        {
            vfxTimer = vfxDuration;
            enemyList.Clear();
            CastAbility();
            if (shouldShakeCamera)
                PlayerManager.Instance.PlayerStateMachine.CinemachineImpulseSource.GenerateImpulse();

        }
        private void Update()
        {
            vfxTimer -= Time.deltaTime;
            if (vfxTimer <= 0f)
            {
                gameObject.SetActive(false);
                return;
            }
        }
        private void CastAbility()
        {
            Debug.Log("Casting ThunderShock");
            Collider[] hits = Physics.OverlapSphere(transform.position, spellMaxRadius, enemyLayer);
            foreach (Collider enemy in hits)
            {
                if (!enemyList.Contains(enemy))
                {
                    KnockbackEnemies(enemy);
                    ApplyDamage(enemy);
                    enemyList.Add(enemy);
                }
            }
        }
        private void KnockbackEnemies(Collider enemy)
        {
            if (enemy.gameObject.TryGetComponent<ForceReceiver>(out var forceReceiver)
                && enemy.gameObject.TryGetComponent<EnemyStateMachine>(out var enemyStateMachine))
            {
                if (enemyStateMachine.IsEnraged)
                    return;
                Vector3 knockBackDir = (enemy.transform.position - this.transform.position).normalized;
                forceReceiver.AddForce(knockBackDir * pushForce);
                enemyStateMachine.ChangeState(new EnemyKnockbackState(enemyStateMachine, 0.5f));
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

