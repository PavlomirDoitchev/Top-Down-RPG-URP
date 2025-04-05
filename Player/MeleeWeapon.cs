using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public WeaponDataSO EquippedWeaponDataSO;
    int baseDamage;
    private List<Collider> enemyColliders = new List<Collider>();
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == 3) return; //Ignore if weapon is set to the Inactive layer
        if (enemyColliders.Contains(other)) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyColliders.Add(other);
            other.BroadcastMessage("TakeDamage", baseDamage, SendMessageOptions.DontRequireReceiver);
            if (PlayerManager.Instance.PlayerStateMachine._PlayerStats.GetResourceType() == CharacterLevelSO.ResourceType.Rage)
            {
                PlayerManager.Instance.PlayerStateMachine._PlayerStats.RegainResource(3);
            }
            //Debug.Log($"Enemy colliders: {enemyColliders.Count}");
            //EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            //if (enemy != null)
            //{
                
            //    enemy.TakeDamage(baseDamage);
            //}
        }
    }
    public void MeleeWeaponDamage(int baseDamage, float multiplier, int index)
    {
        this.baseDamage = Mathf.RoundToInt(baseDamage * multiplier);
    }
    public void ClearHitEnemies()
    {
        enemyColliders.Clear();
        //Debug.Log($"Enemy colliders {enemyColliders.Count}");
    }
    
}
