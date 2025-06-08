using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
public class MeleeWeapon : MonoBehaviour
{
    private int baseDamage;
    public WeaponDataSO EquippedWeaponDataSO;
    [SerializeField] string targetLayerName;
    [SerializeField] int ignoreInactiveLayer = 3; //In the Unity Editor, this is the Inactive layer for the player
    [SerializeField] DamageNumber damageText;
    [SerializeField] TrailRenderer trailRenderer;
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
                damageText.Spawn(other.transform.position, baseDamage);
            }
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
    public void TrailRenderSwitcher()
    {
        trailRenderer.emitting = !trailRenderer.emitting;
    }
}
