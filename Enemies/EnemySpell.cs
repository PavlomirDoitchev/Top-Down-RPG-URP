
using Assets.Scripts.Combat_Logic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemySpell : MonoBehaviour
    {
        public ProjectilePool projectilePool;
        [SerializeField] Transform spawnPosition;
        public void Cast(Transform target) 
        {
            GameObject projectile = projectilePool.GetProjectile();
            projectile.transform.position = spawnPosition.transform.position;
            projectile.transform.rotation = Quaternion.identity;
            var projectileComponent = projectile.GetComponent<Fireball>();
            projectileComponent.Initialize(target);
        }
    }
}
