using UnityEngine;
using StylizedWater3.DynamicEffects;
using StylizedWater3;

namespace Assets.Scripts.Utility.EnvironmentFX
{
    [RequireComponent(typeof(ParticleTrailEmitter))]
    public class WaterTrailController : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Height above water surface to spawn ripples")]
        public float heightOffset = 0.05f;

        private ParticleTrailEmitter trailEmitter;

        void Awake()
        {
            trailEmitter = GetComponent<ParticleTrailEmitter>();
        }

        void Update()
        {
            WaterObject water = WaterObject.Find(transform.position, false);

            if (water)
            {
                float waterHeight = water.transform.position.y; 

                if (transform.position.y <= waterHeight + heightOffset)
                {
                    EnableTrail(true);
                    return;
                }
            }

            EnableTrail(false);
        }

        public void EnableTrail(bool enable)
        {
            if (trailEmitter != null && trailEmitter.particleSystem != null)
            {
                var emission = trailEmitter.particleSystem.emission;
                emission.enabled = enable;
            }
        }
    }
}
