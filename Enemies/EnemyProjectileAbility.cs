
using Assets.Scripts.Combat_Logic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyProjectileAbility : MonoBehaviour
    {
        //[SerializeField] int projectileCount = 1;
        //[SerializeField] float spreadAngle = 15f; 
        public ProjectilePool projectilePool;
        [SerializeField] Transform spawnPosition;
        public void Cast(Transform target)
        {
            GameObject projectile = projectilePool.GetProjectile();
            projectile.transform.position = spawnPosition.transform.position;
            projectile.transform.rotation = Quaternion.identity;
            var projectileComponent = projectile.GetComponent<ProjectileSpell>();
            projectileComponent.Initialize(target);
            projectile.SetActive(true);
            //if(projectileCount <= 0) return;
            //if(projectileCount > 1)
            //{
            //    CastMultiple(target);
            //    return;
            //}
            // Cast a single projectile
        }
        //public void CastMultiple(Transform target)
        //{
        //    int numProjectiles = projectileCount;

        //    for (int i = 0; i < numProjectiles; i++)
        //    {
        //        GameObject projectile = projectilePool.GetProjectile();
        //        projectile.transform.position = spawnPosition.position;
        //        projectile.transform.rotation = Quaternion.identity;

        //        Vector3 directionToTarget = (target.position - spawnPosition.position).normalized;

        //        float angleOffset = (i - (numProjectiles - 1) / 2f) * spreadAngle;
        //        Quaternion rotationOffset = Quaternion.AngleAxis(angleOffset, Vector3.up); 
        //        Vector3 spreadDir = rotationOffset * directionToTarget;

        //        var projectileComponent = projectile.GetComponent<EnemyProjectileSpell>();
        //        projectileComponent.Initialize(target, spreadDir * 0.5f);
        //    }
        //}
    }
}