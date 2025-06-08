using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
public class MeleeWeapon : MonoBehaviour
{
    private int baseDamage;
    private bool shouldKnockback;
    public WeaponDataSO EquippedWeaponDataSO;
    [SerializeField] string targetLayerName;
    [SerializeField] int ignoreInactiveLayer = 3; //In the Unity Editor, this is the Inactive layer for the player
    [SerializeField] DamageNumber damageText;
    [SerializeField] TrailRenderer trailRenderer;
    private readonly List<Collider> enemyColliders = new List<Collider>();
    PlayerManager playerManager;
    private void Start()
    {
        playerManager = PlayerManager.Instance;
    }
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
                if (other.TryGetComponent<ForceReceiver>(out var forceReceiver)
                    && other.TryGetComponent<EnemyStateMachine>(out var enemyStateMachine)
                    && shouldKnockback)
                {
                    Vector3 knockbackDir = (other.transform.position - playerManager.PlayerStateMachine.transform.position).normalized;
                    forceReceiver.AddForce(knockbackDir * 50f);

                    // Switch to knockback state
                    enemyStateMachine.ChangeState(new EnemyKnockbackState(enemyStateMachine, 0.5f)); // adjust duration
                }
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
    public void ShouldKnockBackSwitcher() 
    {
        
        shouldKnockback = !shouldKnockback;
    }
}
