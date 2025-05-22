using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public WeaponDataSO EquippedWeaponDataSO;
    [SerializeField] string targetLayerName;
    [SerializeField] int ignoreInactiveLayer = 3; //In the Unity Editor, this is the Inactive layer for the player
    private int baseDamage;
    private readonly List<Collider> enemyColliders = new List<Collider>();
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == ignoreInactiveLayer) return; 
        if (enemyColliders.Contains(other)) return;

        if (other.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            enemyColliders.Add(other);
            IDamagable damagable = other.GetComponent<IDamagable>();
            if(damagable != null)
            {
                damagable.TakeDamage(baseDamage);
            }
            //other.BroadcastMessage("TakeDamage", baseDamage, SendMessageOptions.DontRequireReceiver);
            //if (PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetResourceType() == CharacterLevelSO.ResourceType.Rage)
            //{
            //    PlayerManager.Instance.PlayerStateMachine.PlayerStats.RegainResource(3);
            //}
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
