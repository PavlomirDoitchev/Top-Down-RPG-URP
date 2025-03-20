using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    int baseDamage;
    public LayerMask enemyLayer;
    private List<Collider> enemyColliders = new List<Collider>();
    //private void OnEnable()
    //{
    //    enemyColliders.Clear();
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == 3) return; //Ignore if weapon is set to the Inactive layer
        if (enemyColliders.Contains(other)) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyColliders.Add(other);
            //Debug.Log($"Enemy colliders: {enemyColliders.Count}");
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
    public void ClearHitEnemies()
    {
        enemyColliders.Clear();
        //Debug.Log($"Enemy colliders {enemyColliders.Count}");
    }
}
