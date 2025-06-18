using Assets.Scripts.Combat_Logic;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Player.Abilities.Fighter_Abilities
{
    public class Fireball : MonoBehaviour
    {

        [SerializeField] float radius = 2f;
        [SerializeField] float damageCheckInterval;
        float timer;
        private readonly List<Collider> enemyColliders = new();
        int damage;
        float multiplier;
        int layerMask = 1 << 7;
        private void OnEnable()
        {
            transform.position = SkillManager.Instance.AimSpell();
            enemyColliders.Clear();
            timer = damageCheckInterval;
        }

        private void OnDisable()
        {
            transform.position = Vector3.zero;
        }
        private void Update()
        {
            timer -= Time.deltaTime;
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);
            if (timer <= 0f && hits.Length > 0)
            {
                foreach (Collider hit in hits)
                {
                    if (enemyColliders.Contains(hit)) continue;
                    enemyColliders.Add(hit);

                    ApplyDamageTo(hit);
                }
                timer = damageCheckInterval;
                enemyColliders.Clear();
            }

        }

        private void ApplyDamageTo(Collider other)
        {
            if (other.gameObject.TryGetComponent<IDamagable>(out var damagable))
            {
                PlayerManager.Instance.PlayerStateMachine.ApplyStatusEffect(other, PlayerManager.Instance.PlayerStateMachine.Ability_Three_Data,
                    PlayerManager.Instance.PlayerStateMachine.Ability_Three_Rank, 1);

                multiplier = PlayerManager.Instance.PlayerStateMachine.Ability_Three_Data[PlayerManager.Instance.PlayerStateMachine.Ability_Three_Rank].damageMultiplier;

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

                //Debug.Log($"Cone hit: {other.gameObject.name}");
                //Debug.Log(enemyList.Count + " enemies hit by cone.");
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
