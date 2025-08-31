using Assets.Scripts.Combat_Logic;
using UnityEngine;

public class Destructable : MonoBehaviour, IDamagable
{
    Rigidbody[] rb;
    public int health = 1;
    private bool isDestroyed = false;

    private void Start()
    {
        rb = GetComponentsInChildren<Rigidbody>();
    }

    public void TakeDamage(int damage, bool applyImpulse)
    {
        if (isDestroyed) return;

        health -= damage;

        if (health <= 0 && rb != null)
        {
            isDestroyed = true;
            foreach (var r in rb)
            {
                r.useGravity = true;
                r.isKinematic = false;
                r.gameObject.layer = LayerMask.NameToLayer("IgnoreAllCollisions");
                r.AddForce(Vector3.up 
                    * Random.Range(1f, 100f) + Vector3.forward * Random.Range(-50f, 50f) + Vector3.right * Random.Range(-50f, 50f));
                //r.AddExplosionForce(Random.Range(1f, 300f), transform.position, 5f);
            }
            this.gameObject.layer = LayerMask.NameToLayer("IgnoreAllCollisions");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyDamage") ||
            other.gameObject.layer == LayerMask.NameToLayer("DamageEnemy"))
        {
            TakeDamage(1, true);
        }
    }
}
