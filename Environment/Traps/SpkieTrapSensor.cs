using Assets.Scripts.Combat_Logic;
using UnityEngine;
using DamageNumbersPro;
public class SpkieTrapSensor : MonoBehaviour
{
    [SerializeField] GameObject movingSpikesReference;
    [SerializeField] private int damageAmount;
    [Header("Timer should equal the cooldown!")]
    [SerializeField] float timer = 1;
    [SerializeField] float spikeCooldown = 1f;
    [SerializeField] float spikeRiseLimit = 0.5f;
    [SerializeField] private float knockUpForce = 5f;
    [SerializeField] private DamageNumber damageText;
    [SerializeField] private GameObject spikeParticleSystem;
    private float spikeOrignalHeight = -1f;
    bool hasTakenDamage = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out var damagable))
        {
            Vector3 spikePosition = movingSpikesReference.transform.localPosition;
            timer -= Time.deltaTime;
            if (timer <= 0 && !hasTakenDamage)
            {
                movingSpikesReference.transform.localPosition = new Vector3(spikePosition.x, spikeRiseLimit, spikePosition.z);
                spikeParticleSystem.gameObject.SetActive(true);
                damagable.TakeDamage(damageAmount, true);
                damageText.Spawn(other.transform.position, damageAmount);
                hasTakenDamage = true;
                if (other.TryGetComponent<ForceReceiver>(out var forceReceiver) && other.gameObject.layer == LayerMask.NameToLayer("MyOutlines"))
                {
                    forceReceiver.AddForce(Vector3.up * knockUpForce);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out var damagable))
        {
            Vector3 spikePosition = movingSpikesReference.transform.localPosition;
            spikePosition.y = spikeOrignalHeight;
            spikeParticleSystem.gameObject.SetActive(false);
            movingSpikesReference.transform.localPosition = spikePosition;
            if (timer <= spikeCooldown)
                timer = spikeCooldown;
            hasTakenDamage = false;
        }
    }
}
