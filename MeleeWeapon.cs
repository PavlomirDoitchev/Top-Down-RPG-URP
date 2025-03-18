using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    int baseDamage;
    public float attackrange;
    public LayerMask enemyLayer;
    [SerializeField] Collider myCollider;
    private List<Collider> enemyColliders = new List<Collider>();
    private void OnEnable()
    {
        enemyColliders.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) return;
        if (enemyColliders.Contains(other)) return;

        enemyColliders.Add(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) 
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(baseDamage);
            }
        }
    }
    public void SetDamage(int baseDamage, float strengthMultiplier)
    {
        this.baseDamage = Mathf.RoundToInt(baseDamage * strengthMultiplier);
    }
}
