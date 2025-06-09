using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat_Logic
{
    public class ProjectilePool : MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        [SerializeField] int poolSize;

        [SerializeField] private List<GameObject> projectiles;

        private void Awake()
        {
            projectiles = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject projectile = Instantiate(ProjectilePrefab);
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

            // If no inactive projectile is found, instantiate a new one
            GameObject newProjectile = Instantiate(ProjectilePrefab);
            projectiles.Add(newProjectile);
            return newProjectile;
        }
    }
}
