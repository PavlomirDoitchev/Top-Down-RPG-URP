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
    float knockbackForce;
    float knockbackDuration = 0.5f; 
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
            if (damagable != null)
            {
                damagable.TakeDamage(baseDamage, false);
                SpawnDamageText(other);
                TryKnockbackEnemy(other);
            }
        }
    }
    /// <summary>
    /// Knock back the enemy if the player is in a state that allows it and the enemy is not enraged.
    /// </summary>
    /// <param name="other"></param>
    private void TryKnockbackEnemy(Collider other)
    {
        if (other.TryGetComponent<ForceReceiver>(out var forceReceiver)
                            && other.TryGetComponent<EnemyStateMachine>(out var enemyStateMachine)
                            && shouldKnockback
                            && !enemyStateMachine.IsEnraged)
        {
            if (playerManager.PlayerStateMachine.PlayerCurrentState is FighterAbilityQState /*add other melee abilities knockback*/)
            {
                knockbackForce = playerManager.PlayerStateMachine.qAbilityData[playerManager.PlayerStateMachine.QAbilityRank].knockbackForce;
            }
            else
            {
                knockbackForce = 50f; //for debugging only
            }
            Vector3 knockbackDir = (other.transform.position - playerManager.PlayerStateMachine.transform.position).normalized;
            forceReceiver.AddForce(knockbackDir * knockbackForce);
            enemyStateMachine.ChangeState(new EnemyKnockbackState(enemyStateMachine, knockbackDuration));
        }
    }

    private void SpawnDamageText(Collider other)
    {
        damageText.Spawn(other.transform.position, baseDamage);
    }

    public void MeleeWeaponDamage(int baseDamage, float multiplier, int index)
    {
        this.baseDamage = Mathf.RoundToInt(baseDamage * (multiplier * .2f));
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
