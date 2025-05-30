using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] float timer = 1;
    [SerializeField] float spikeCooldown = 1f;
    [SerializeField] GameObject spikes;
    [SerializeField] float spikeHeight = 0.5f;
    private float spikeOrignalHeight = -0.266f;
    bool hasTakenDamage = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out var damagable))
        {
            Vector3 spikePosition = spikes.transform.localPosition;
            timer -= Time.deltaTime;
            if (timer <= 0 && !hasTakenDamage)
            {
                spikes.transform.localPosition = new Vector3(spikePosition.x, spikeHeight, spikePosition.z);
                damagable.TakeDamage(damage);
                hasTakenDamage = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out var damagable))
        {
            Vector3 spikePosition = spikes.transform.localPosition;
            spikePosition.y = spikeOrignalHeight;
            spikes.transform.localPosition = spikePosition;
            if (timer <= spikeCooldown)
                timer = spikeCooldown;
            hasTakenDamage = false;
        }
    }
}
