using Assets.Scripts.Combat_Logic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] GameObject movingSpikesReference;
    [SerializeField] private int damage;
    [Header("Timer should equal the cooldown!")]
    [SerializeField] float timer = 1;
    [SerializeField] float spikeCooldown = 1f;
    [SerializeField] float spikeRiseLimit = 0.5f;
    private float spikeOrignalHeight = -0.266f;
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
                damagable.TakeDamage(damage);
                hasTakenDamage = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out var damagable))
        {
            Vector3 spikePosition = movingSpikesReference.transform.localPosition;
            spikePosition.y = spikeOrignalHeight;
            movingSpikesReference.transform.localPosition = spikePosition;
            if (timer <= spikeCooldown)
                timer = spikeCooldown;
            hasTakenDamage = false;
        }
    }
}
