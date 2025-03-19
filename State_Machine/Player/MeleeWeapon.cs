using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    int baseDamage;
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
    public void MeleeWeaponDamage(int baseDamage, float strengthMultiplier, int index)
    {
        this.baseDamage = Mathf.RoundToInt(baseDamage * strengthMultiplier);
    }
   
}
