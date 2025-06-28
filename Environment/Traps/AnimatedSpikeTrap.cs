using Assets.Scripts.Combat_Logic;
using UnityEngine;

namespace Assets.Scripts.Environment.Traps
{
    public class AnimatedSpikeTrap : MonoBehaviour
    {


        [Tooltip("If true, animation will loop, else trggier upon something landing on trap")]
        [SerializeField] bool isAutomatic;
        [SerializeField] float timeToActivate = 1f;
        [SerializeField] private float timer; // Timer to control the activation delay
        private Animator animator;
        [SerializeField] private BoxCollider damageCollider;
        bool hasEnteredTrigger = false;
        private void Start()
        {
            if (damageCollider == null)
                damageCollider = GetComponentInChildren<BoxCollider>();
            animator = GetComponent<Animator>();
            if (isAutomatic)
                animator.Play("LoopingSpikes");
            else
                animator.Play("IdleSpikes");

            timer = timeToActivate;

        }
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<IDamagable>(out var damagable) && !isAutomatic)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f && !hasEnteredTrigger)
                {
                    hasEnteredTrigger = true;

                    animator.Play("TriggerSpikes");
                    //timer = timeToActivate;
                }

            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IDamagable>(out var damagable) && !isAutomatic)
            {
                hasEnteredTrigger = false;


                timer = timeToActivate; // Reset timer

            }

        }
        /// <summary>
        /// Used in animation events
        /// </summary>
        public void EnableCollider()
        {
            damageCollider.enabled = !damageCollider.enabled;
        }
    }
}
