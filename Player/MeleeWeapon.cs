using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
public class MeleeWeapon : MonoBehaviour
{
    //TODO: Remove everything from playerstatemachine regarding levels, abilities, and ranks.
    //TODO: Move all melee weapon calculations to this class.
    //TODO: Add another ability that does not require a weapon. frontal cone AoE attack that stuns enemies in front of the player and deals damage.
    //TODO: Simplify damage calculations.
    //TODO: Start handling attack related stuff in AnimationEvents instead of in each state.
    //TODO: Remove inputs from Locomotion State and move them to a new class that handles the input for the player.
    //TODO: Implement weapon switching.
    private readonly List<Collider> enemyColliders = new List<Collider>();
    int ignoreInactiveLayer = 3;
    string targetLayerName = "Enemy";
    [SerializeField] private PlayerMelee _playerMelee;
    private PlayerManager _playerManager;
    private void Start()
    {
        _playerManager = PlayerManager.Instance;
    }
    private void Update()
    {
        //create a new class that handles the input for the player like this. checking each state.
        //if (Input.GetKeyDown(KeyCode.K) && playerManager.PlayerStateMachine.PlayerCurrentState is FighterLocomotionState) 
        //{
        //    playerManager.PlayerStateMachine.EquipNewWeapon(EquippedWeaponDataSO.weaponPrefab);
        //}
        //if (playerManager.PlayerStateMachine.PlayerCurrentState is FighterBasicAttackChainOne ||
        //   playerManager.PlayerStateMachine.PlayerCurrentState is FighterBasicAttackChainTwo ||
        //   playerManager.PlayerStateMachine.PlayerCurrentState is FighterBasicAttackChainThree)
        //{
        //    damageColliders[0].layer = LayerMask.NameToLayer(targetLayerName);
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == ignoreInactiveLayer) return;
        if (enemyColliders.Contains(other)) return;

        if (other.gameObject.layer == LayerMask.NameToLayer(targetLayerName)
            && this.gameObject.layer == 7
            && other.gameObject.TryGetComponent<IDamagable>(out var damagable))
        {
            enemyColliders.Add(other);
            //Debug.Log(enemyColliders.Count + " enemies hit");

            damagable.TakeDamage(_playerMelee.EquippedWeaponDataSO.maxDamage, false);
            _playerMelee.SpawnDamageText(other);
            TryKnockbackEnemy(other);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        ClearHitEnemies(other);
    }
    /// <summary>
    /// Knock back the enemy if the player is in a state that allows it and the enemy is not enraged.
    /// </summary>
    /// <param name="other"></param>
    private void TryKnockbackEnemy(Collider other)
    {
        if (other.TryGetComponent<ForceReceiver>(out var forceReceiver)
                            && other.TryGetComponent<EnemyStateMachine>(out var enemyStateMachine)
                            && _playerMelee.ShouldKnockback
                            && !enemyStateMachine.IsEnraged)
        {
            if (_playerManager.PlayerStateMachine.PlayerCurrentState is FighterAbilityQState /*add other melee abilities knockback*/)
            {
                _playerMelee.KnockbackForce = _playerManager.PlayerStateMachine.qAbilityData[_playerManager.PlayerStateMachine.QAbilityRank].knockbackForce;
            }
            else
            {
                _playerMelee.KnockbackForce = 50f; //for debugging only
            }
            Vector3 knockbackDir = (other.transform.position - _playerManager.PlayerStateMachine.transform.position).normalized;
            forceReceiver.AddForce(knockbackDir * _playerMelee.KnockbackForce);
            enemyStateMachine.ChangeState(new EnemyKnockbackState(enemyStateMachine, _playerMelee.KnockbackDuration));
        }
    }

    public void ClearHitEnemies(Collider other)
    {
        if (enemyColliders.Contains(other))
        {
            enemyColliders.Remove(other);
        }
    }
}
