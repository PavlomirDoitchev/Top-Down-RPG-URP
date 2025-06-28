using Assets.Scripts.Combat_Logic;
using UnityEngine;
using System;
namespace Assets.Scripts.Player.Weapons
{
    [Serializable]
    public class PlayerProjectileAbility : Skills
    {
        [SerializeField] private string projectileTag;
        [SerializeField] private Transform spawnPoint;

        public override void UseSkill()
        {
            base.UseSkill();
        }

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
