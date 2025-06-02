using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using UnityEngine;

public class MovingPlatformDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float knockbackForce = 5f;
    //[SerializeField] private float timer = 1f;
    //bool hasTakenDamage = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out var damagable))
        {
            //timer -= Time.deltaTime;

            damagable.TakeDamage(damage);
            //hasTakenDamage = true;
            if(other.TryGetComponent<ForceReceiver>(out var forceReceiver))
            {
                forceReceiver.AddForce((other.transform.position - transform.position).normalized * knockbackForce);
            }
        }
    }
}
