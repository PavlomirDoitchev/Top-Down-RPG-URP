
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class TestEvents : MonoBehaviour
    {
        [SerializeField] ParticleSystem attackEffect;
        Animator animator;
        private void Start()
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on the GameObject.");
            }

        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                animator.Play("Combat_Unarmed_Attack");
            }
            
        }
        public void SpawnVFX()
        {
            if (!attackEffect.isPlaying)
                attackEffect.Play();
        }
    }
}
