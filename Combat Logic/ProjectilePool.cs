using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat_Logic
{
    public class ProjectilePool : MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        [SerializeField] int poolSize;
        [SerializeField] Transform projectileContainer;

        [SerializeField] private List<GameObject> projectiles;

        private void Awake()
        {
            projectiles = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject projectile = Instantiate(ProjectilePrefab, projectileContainer);
                projectile.SetActive(false);
                projectiles.Add(projectile);
            }
        }

        public GameObject GetProjectile()
        {
            foreach (var projectile in projectiles)
            {
                if (!projectile.activeInHierarchy)
                {
                    projectile.SetActive(true);
                    return projectile;
                }
            }
            return AddMoreProjectiles();
        }

        private GameObject AddMoreProjectiles()
        {
            GameObject newProjectile = Instantiate(ProjectilePrefab, projectileContainer);
            projectiles.Add(newProjectile);
            return newProjectile;
        }
    }
}
