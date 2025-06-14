using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using Assets.Scripts.State_Machine.Player_State_Machine;

public class Shockwave : MonoBehaviour
{
    [SerializeField] bool shouldShakeCamera = true;

    private readonly List<Collider> enemyList = new();

    float knockBackForce;
    int damage;
    float multiplier;

    [Header("Cone Settings")]
    [SerializeField] float maxRadius = 5f;
    [SerializeField] float coneAngle = 60f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float damageCheckInterval = 0.1f;

    [Header("Spell Settings")]
    //[SerializeField] Collider spellCollider;  // You might still want this for visuals or triggers?
    [SerializeField] float maxDuration = 1f;
    float timer;

    private void OnEnable()
    {
        timer = maxDuration;
        enemyList.Clear();

        if (shouldShakeCamera)
            PlayerManager.Instance.PlayerStateMachine.CinemachineImpulseSource.GenerateImpulse();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // Spell finished, disable or reset as needed
            gameObject.SetActive(false);
        }
        else
        {
            // Periodic cone collision check
            damageCheckInterval -= Time.deltaTime;
            if (damageCheckInterval <= 0f)
            {
                CheckConeCollision();
                damageCheckInterval = 0.1f; // reset interval
            }
        }
    }

    private void CheckConeCollision()
    {
        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;

        Collider[] hits = Physics.OverlapSphere(origin, maxRadius, enemyLayer);
        foreach (Collider hit in hits)
        {
            if (enemyList.Contains(hit))
                continue;

            Vector3 toTarget = (hit.transform.position - origin).normalized;

            float angle = Vector3.Angle(forward, toTarget);
            if (angle <= coneAngle * 0.5f)
            {
                enemyList.Add(hit);
                TryKnockbackEnemy(hit);
                ApplyDamageTo(hit);
            }
        }
    }

    private void ApplyDamageTo(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamagable>(out var damagable))
        {
            multiplier = PlayerManager.Instance.PlayerStateMachine.Ability_Two_Data[PlayerManager.Instance.PlayerStateMachine.Ability_Two_Rank].damageMultiplier;

            if (PlayerManager.Instance.PlayerStateMachine.CriticalStrikeSuccess())
            {
                damage = Mathf.RoundToInt(PlayerManager.Instance.PlayerStateMachine.WeaponDamage(damage, multiplier)
                    * (PlayerManager.Instance.PlayerStateMachine.PlayerStats.CriticalDamageModifier
                    + PlayerManager.Instance.PlayerStateMachine.Weapon.criticalDamageModifier));
                damagable.TakeDamage(damage, false);
                PlayerManager.Instance.PlayerStateMachine.DamageText[4].Spawn(other.transform.position + Vector3.up * 2f, damage);
            }
            else
            {
                damage = Mathf.RoundToInt(PlayerManager.Instance.PlayerStateMachine.WeaponDamage(damage, multiplier));
                damagable.TakeDamage(damage, false);
                PlayerManager.Instance.PlayerStateMachine.DamageText[3].Spawn(other.transform.position + Vector3.up * 2f, damage);
            }

            Debug.Log($"Cone hit: {other.gameObject.name}");
            Debug.Log(enemyList.Count + " enemies hit by cone.");
        }
    }

    private void TryKnockbackEnemy(Collider other)
    {
        if (other.TryGetComponent<ForceReceiver>(out var forceReceiver)
            && other.TryGetComponent<EnemyStateMachine>(out var enemyStateMachine)
            && !enemyStateMachine.IsEnraged)
        {
            if (!PlayerManager.Instance.PlayerStateMachine.Ability_Two_Data[PlayerManager.Instance.PlayerStateMachine.Ability_Two_Rank].canKnockback)
                return;

            knockBackForce = PlayerManager.Instance.PlayerStateMachine.Ability_Two_Data[PlayerManager.Instance.PlayerStateMachine.Ability_Two_Rank].knockbackForce;

            Vector3 knockbackDir = (other.transform.position - PlayerManager.Instance.PlayerStateMachine.transform.position).normalized;
            forceReceiver.AddForce(knockbackDir * knockBackForce);
            enemyStateMachine.ChangeState(new EnemyKnockbackState(enemyStateMachine, .5f));
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, maxRadius);

        int stepCount = 30;
        float angleStep = coneAngle / stepCount;
        Quaternion startRotation = Quaternion.AngleAxis(-coneAngle / 2f, Vector3.up);

        for (int i = 0; i <= stepCount; i++)
        {
            Quaternion rotation = startRotation * Quaternion.AngleAxis(i * angleStep, Vector3.up);
            Vector3 direction = rotation * forward;

            Gizmos.DrawLine(origin, origin + direction * maxRadius);
        }
    }
#endif
}
