using UnityEngine;
using StylizedWater3.DynamicEffects;
using StylizedWater3;

namespace Assets.Scripts.Utility.EnvironmentFX
{
    [RequireComponent(typeof(ParticleTrailEmitter))]

    public class WaterTrailController : MonoBehaviour
    {
        public float heightOffset = 0.05f;

        private ParticleTrailEmitter trailEmitter;
        //[SerializeField] DynamicEffect impactRipple;
        private void Awake()
        {
            trailEmitter = GetComponent<ParticleTrailEmitter>();
        }

        private void Update()
        {
            WaterObject water = WaterObject.Find(transform.position, false);

            if (water)
            {
                float waterHeight = 0f;
               
                    if (transform.position.y <= waterHeight + heightOffset)
                    {
                        EnableTrail(true);
                        //impactRipple.renderer.enabled = true;
                    return;
                    }
                
            }
            //impactRipple.renderer.enabled = false;
            EnableTrail(false);
        }

        private void EnableTrail(bool enable)
        {
            if (trailEmitter != null && trailEmitter.particleSystem != null)
            {
                var emission = trailEmitter.particleSystem.emission;
                emission.enabled = enable;
            }
        }
    }
}
