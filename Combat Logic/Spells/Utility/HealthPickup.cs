using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Combat_Logic.Spells.Utility
{
    public class HealthPickup : MonoBehaviour
    {
        private int healthToRestore;
        [SerializeField] float rotationSpeed = 50f;
        [SerializeField] float bobbingAmplitude = 0.25f;
        [SerializeField] float bobbingFrequency = 1f;
        [SerializeField] float timeBeforeDespawn = 20f;
        [SerializeField] float percentageHealthToRestore = 0.1f;
        float timer = 0f;
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= timeBeforeDespawn)
            {
                Destroy(gameObject);
            }
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            float newY = Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("MyOutlines"))
            {
                healthToRestore = Mathf.RoundToInt(PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetMaxHealth() * percentageHealthToRestore);
                PlayerManager.Instance.PlayerStateMachine.PlayerStats.Heal(healthToRestore);
                Destroy(gameObject);
            }
        }
    }
}
