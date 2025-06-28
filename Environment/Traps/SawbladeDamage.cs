using Assets.Scripts.Combat_Logic;
using UnityEngine;

namespace Assets.Scripts.Environment.Traps
{
    public class SawbladeDamage : MonoBehaviour
    {
        [SerializeField] private int damage;
        [Tooltip("Leave empty for raw damage")]
        [SerializeField] private StatusEffectData statusEffectData;

        [SerializeField] private float knockbackForce = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDamagable>(out var damagable))
            {
                //timer -= Time.deltaTime;
                if (other.TryGetComponent<IEffectable>(out var effectable) && statusEffectData != null)
                {
                    effectable.ApplyEffect(statusEffectData);
                }
                damagable.TakeDamage(damage, true);
                //hasTakenDamage = true;
                if (other.TryGetComponent<ForceReceiver>(out var forceReceiver) && other.gameObject.layer == LayerMask.NameToLayer("MyOutlines"))
                {
                    forceReceiver.AddForce((other.transform.position - transform.position).normalized * knockbackForce);
                }
            }
        }
    }
}
