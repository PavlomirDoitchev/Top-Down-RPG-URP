using Assets.Scripts.Combat_Logic;
using UnityEngine;

namespace Assets.Scripts.Environment.Traps
{
    public class SpikeTrapDamage : MonoBehaviour
    {
        [SerializeField] int damage = 100;
        [Tooltip("Leave empty for raw damage")]
        [SerializeField] StatusEffectData statusEffectData;
        [SerializeField] float knockbackForce = 15f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.TakeDamage(damage, true);

                if (other.TryGetComponent<IEffectable>(out var effectable) && statusEffectData != null)
                    effectable.ApplyEffect(statusEffectData);

                if (other.TryGetComponent<ForceReceiver>(out var forceReceiver) && other.gameObject.layer == LayerMask.NameToLayer("MyOutlines"))
                    forceReceiver.AddForce((other.transform.position - transform.position).normalized * knockbackForce);
            }
        }
    }
}
