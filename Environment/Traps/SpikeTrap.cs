using Assets.Scripts.Player;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private int damage;
    [SerializeField] float timer = 1;
    [SerializeField] Transform spikes;
    private void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 spikePosition = spikes.transform.localPosition;
            var playerStats = PlayerManager.Instance.PlayerStateMachine.PlayerStats;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                spikePosition.y = Vector3.Slerp(spikePosition, new Vector3(spikePosition.x, spikePosition.y + 0.25f, spikePosition.z), Time.deltaTime * 2).y;
                
            }


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 spikePosition = spikes.transform.localPosition;
            spikePosition.y = -0.266f;
            spikes.transform.localPosition = spikePosition;
            timer = 1;
        }
    }
}
