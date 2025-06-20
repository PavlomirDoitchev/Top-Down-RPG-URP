using Assets.Scripts.Combat_Logic;
using UnityEngine;

namespace Assets.Scripts.Player.Weapons
{
    public class PlayerProjectileAbility : MonoBehaviour
    {
        [SerializeField] private string projectileTag;
        [SerializeField] private Transform spawnPoint;

        public void Cast()
        {
            GameObject projectile = ProjectilePoolManager.Instance.GetProjectile(projectileTag);
            projectile.transform.position = spawnPoint.position;
            projectile.transform.rotation = Quaternion.identity;

            if (projectile.TryGetComponent<PlayerProjectile>(out var proj))
            {
                proj.Init();

            }
           
            projectile.SetActive(true);
        }
    }
}
