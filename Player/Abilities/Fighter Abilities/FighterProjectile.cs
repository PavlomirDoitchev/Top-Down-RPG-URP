using Assets.Scripts.Combat_Logic;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using UnityEngine;  
namespace Assets.Scripts.Player.Abilities.Fighter_Abilities
{
    public class FighterProjectile : PlayerProjectile
    {
        bool isEmpowered = false;
        [SerializeField] float stunDuration = 2f;
        int damage;
        private void OnEnable()
        {
            
            if (PlayerManager.Instance.PlayerStateMachine.PlayerStats != null
                   && PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentResource() >= 50)
            {
                PlayerManager.Instance.PlayerStateMachine.PlayerStats.UseResource(50);
                isEmpowered = true;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground")
                || other.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                if (spellHitPrefab != null)
                    Instantiate(spellHitPrefab, this.transform.position, Quaternion.identity);
                Debug.Log($"Projectile hit {other.gameObject.name}");
                gameObject.SetActive(false);
            }

            else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                
                damage = (projectileData.damage * (isEmpowered ? 2 : 1)) + PlayerManager.Instance.PlayerStateMachine.WeaponDamage(damage, 1);
                if (effectData != null && other.TryGetComponent<IEffectable>(out var effectable))
                {
                    effectable.ApplyEffect(effectData);
                }
                if (projectileData != null && other.TryGetComponent<IDamagable>(out var damagable))
                {
                    if (spellHitPrefab != null)
                        Instantiate(spellHitPrefab, this.transform.position, Quaternion.identity); //Remove later, add to a pool

                    if (isEmpowered)
                    {
                        damagable.TakeDamage(damage, false);
                        PlayerManager.Instance.PlayerStateMachine.PlayerStats.Heal(damage);
                        other.TryGetComponent<EnemyStateMachine>(out var enemy);
                        enemy?.ChangeState(new EnemyStunnedState(enemy, stunDuration));
                        projectileData.healNumberPrefab.Spawn(PlayerManager.Instance.PlayerStateMachine.transform.position, damage);
                        projectileData.damageNumberPrefab.Spawn(other.transform.position, damage);
                    }
                    else
                    {
                        damagable.TakeDamage(damage, false);
                        projectileData.damageNumberPrefab.Spawn(other.transform.position, damage);
                    }
                }
                //Debug.Log($"Projectile hit {other.gameObject.name}");
                gameObject.SetActive(false);
            }

        }
    }
}
