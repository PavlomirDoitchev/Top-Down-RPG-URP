using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using Assets.Scripts.State_Machine.Player_State_Machine.FighterStates;
public class MeleeWeapon : MonoBehaviour
{
	public readonly List<Collider> enemyColliders = new List<Collider>();
	private PlayerManager _playerManager;
	int damage;
	float multiplier;
	float knockBackForce;
    private void Start()
	{
		_playerManager = PlayerManager.Instance;
	}
 
    private void OnTriggerEnter(Collider other)
	{

		if (enemyColliders.Contains(other)) return;
		if (other.gameObject.layer == 7
			//&& this.gameObject.layer == 13
			&& other.gameObject.TryGetComponent<IDamagable>(out var damagable))
		{
			enemyColliders.Add(other);
			TryKnockbackEnemy(other);
			//Debug.Log(enemyColliders.Count + " enemies hit");

			if (_playerManager.PlayerStateMachine.PlayerCurrentState is FighterAbilityOneState) 
			{
				multiplier = _playerManager.PlayerStateMachine.Ability_One_Data[_playerManager.PlayerStateMachine.Ability_One_Rank].damageMultiplier;
				_playerManager.PlayerStateMachine.ApplyStatusEffect(other, _playerManager.PlayerStateMachine.Ability_One_Data, _playerManager.PlayerStateMachine.Ability_One_Rank, 0);
			
			}
			else
			{ 
			
				multiplier = _playerManager.PlayerStateMachine.BasicAttackData[_playerManager.PlayerStateMachine.BasicAttackRank].damageMultiplier;
			}

			if (_playerManager.PlayerStateMachine.CriticalStrikeSuccess())
			{
				damage = Mathf.RoundToInt(_playerManager.PlayerStateMachine.WeaponDamage(damage, multiplier)
					* (_playerManager.PlayerStateMachine.PlayerStats.CriticalDamageModifier + _playerManager.PlayerStateMachine.Weapon.criticalDamageModifier));
				damagable.TakeDamage(damage, false);
				_playerManager.PlayerStateMachine.DamageText[2].Spawn(other.transform.position + Vector3.up * 2f, damage);
			}
			else
			{
				damage = _playerManager.PlayerStateMachine.WeaponDamage(damage, multiplier);
				damagable.TakeDamage(damage, false);
				_playerManager.PlayerStateMachine.DamageText[1].Spawn(other.transform.position, damage);

			}

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
							&& !enemyStateMachine.IsEnraged)
		{
			switch (_playerManager.PlayerStateMachine.PlayerCurrentState)
			{
				case FighterAbilityOneState:
					if (!_playerManager.PlayerStateMachine.Ability_One_Data[_playerManager.PlayerStateMachine.Ability_One_Rank].canKnockback)
						return;
					knockBackForce = _playerManager.PlayerStateMachine.Ability_One_Data[_playerManager.PlayerStateMachine.Ability_One_Rank].knockbackForce;
					break;
				case FighterBasicAttackChainOne:
				case FighterBasicAttackChainTwo:
				case FighterBasicAttackChainThree:
					if (!_playerManager.PlayerStateMachine.BasicAttackData[_playerManager.PlayerStateMachine.BasicAttackRank].canKnockback)
						return;
					knockBackForce = _playerManager.PlayerStateMachine.BasicAttackData[_playerManager.PlayerStateMachine.BasicAttackRank].knockbackForce;
					break;
			}

			Vector3 knockbackDir = (other.transform.position - _playerManager.PlayerStateMachine.transform.position).normalized;
			forceReceiver.AddForce(knockbackDir * knockBackForce);
			enemyStateMachine.ChangeState(new EnemyKnockbackState(enemyStateMachine, .5f));
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
