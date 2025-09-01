using Assets.Scripts.Combat_Logic;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Player.Abilities.Fighter_Abilities
{
    public class LightningBolt : PlayerProjectile
    {
        [SerializeField] float stunDuration = 2f;
        [SerializeField] int maxJumps = 3;
        [SerializeField] float jumpRange = 5f;
        [SerializeField] float jumpDelay = 0.2f;

        bool isEmpowered = false;
        int damage;
        HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
        int currentJump = 0;

        private void OnEnable()
        {
            isEmpowered = false;
            hitEnemies.Clear();
            currentJump = 0;
            isChaining = false;

            if (PlayerManager.Instance.PlayerStateMachine.PlayerStats != null
                && PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentResource() >= 50)
            {
                PlayerManager.Instance.PlayerStateMachine.PlayerStats.UseResource(50);
                isEmpowered = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == null || hitEnemies.Contains(other.gameObject))
                return;

            if (other.gameObject.layer == LayerMask.NameToLayer("Ground")
                || other.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                if (spellHitPrefab != null)
                    Instantiate(spellHitPrefab, transform.position, Quaternion.identity);

                gameObject.SetActive(false);
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                hitEnemies.Add(other.gameObject);

                damage = projectileData.damage + PlayerManager.Instance.PlayerStateMachine.WeaponDamage(damage, 1);

                if (effectData != null && other.TryGetComponent<IEffectable>(out var effectable))
                    effectable.ApplyEffect(effectData);

                if (other.TryGetComponent<IDamagable>(out var damagable))
                {
                    if (spellHitPrefab != null)
                        Instantiate(spellHitPrefab, transform.position, Quaternion.identity);
                    if (other.TryGetComponent<EnemyStateMachine>(out var enemy))
                        enemy.ChangeState(new EnemyStunnedState(enemy, stunDuration));
                    damagable.TakeDamage(damage, false);

                    if (isEmpowered)
                    {
                        PlayerManager.Instance.PlayerStateMachine.PlayerStats.Heal(damage);

                        //if (other.TryGetComponent<EnemyStateMachine>(out var enemy))
                        //    enemy.ChangeState(new EnemyStunnedState(enemy, stunDuration));

                        projectileData.healNumberPrefab.Spawn(PlayerManager.Instance.PlayerStateMachine.transform.position, damage);
                    }

                    projectileData.damageNumberPrefab.Spawn(other.transform.position, damage);

                    if (isEmpowered)
                    {
                        currentJump++;
                        StartCoroutine(JumpToNextEnemy(other.transform.position));
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }

        private IEnumerator JumpToNextEnemy(Vector3 fromPosition)
        {
            if (currentJump >= maxJumps)
            {
                gameObject.SetActive(false);
                yield break;
            }

            yield return new WaitForSeconds(jumpDelay);

            Collider[] nearby = Physics.OverlapSphere(fromPosition, jumpRange, LayerMask.GetMask("Enemy"));
            GameObject nextTarget = null;
            float closestDistance = float.MaxValue;

            foreach (var collider in nearby)
            {
                if (!hitEnemies.Contains(collider.gameObject))
                {
                    float dist = Vector3.Distance(fromPosition, collider.transform.position);
                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        nextTarget = collider.gameObject;
                    }
                }
            }

            if (nextTarget != null)
            {
                isChaining = true;
                Vector3 start = transform.position;
                Vector3 targetPos = nextTarget.transform.position + Vector3.up * 1f;
                float elapsed = 0f;
                float travelTime = 0.15f;

                SetDirection(targetPos - transform.position); // face toward next target

                while (elapsed < travelTime)
                {
                    transform.position = Vector3.Lerp(start, targetPos, elapsed / travelTime);
                    elapsed += Time.deltaTime;
                    yield return null;
                }

                transform.position = targetPos;
                isChaining = false;

                OnTriggerEnter(nextTarget.GetComponent<Collider>());
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
