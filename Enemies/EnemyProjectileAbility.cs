
using Assets.Scripts.Combat_Logic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyProjectileAbility : MonoBehaviour
    {
		[SerializeField] string projectileTag;
		[SerializeField] Transform spawnPosition;

		public void Cast(Transform target)
		{
			GameObject projectile = ProjectilePoolManager.Instance.GetProjectile(projectileTag);
			projectile.transform.position = spawnPosition.position;
			projectile.transform.rotation = Quaternion.identity;
			if (projectile.TryGetComponent<ProjectileSpell>(out var projectileComponent))
			{
				projectileComponent.Initialize(target);
			}
			else
			{
				Debug.LogWarning("Projectile does not have a ProjectileSpell component.");
			}
			projectile.SetActive(true);
		}
		
	}
}