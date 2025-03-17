using UnityEngine;
using System.Collections.Generic;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private List<Collider> alreadyHit = new List<Collider>();
    private void OnEnable()
    {
        alreadyHit.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) return;
        if (alreadyHit.Contains(other)) return;
        alreadyHit.Add(other);
        if (other.TryGetComponent<Health>(out Health currentHealth)) 
        {
            currentHealth.DealDamage(10);
            Debug.Log(currentHealth);
        }
    }
}
