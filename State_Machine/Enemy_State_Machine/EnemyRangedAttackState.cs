using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
	public class EnemyRangedAttackState : EnemyBaseState
	{
		public EnemyRangedAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
		{
		}

		public override void EnterState()
		{
			base.EnterState();
			_enemyStateMachine.Agent.isStopped = true;
			RotateToPlayer();
			if(!_enemyStateMachine.RangedAttackCooldown.IsReady)
				_enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.ChannelAnimationName, .1f);
			
		}

		public override void UpdateState(float deltaTime)
		{
			_enemyStateMachine.RangedAttackCooldown.Tick(deltaTime);
			
			if (CheckForGlobalTransitions()) return;
			RotateToPlayer(deltaTime);
			if (!HasLineOfSight() || OutOfShootingRange())
			{
				_enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
				return;
			}

			if (_enemyStateMachine.RangedAttackCooldown.IsReady)
			{
				_enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.CastAnimationName, .1f);
				_enemyStateMachine.RangedAttackCooldown.Start(_enemyStateMachine.RangedAttackCooldownDuration);
			}
			

			if (ShouldFlee())
			{
				_enemyStateMachine.ChangeState(new EnemyFleeState(_enemyStateMachine));
			}
			else if (ShouldNotFlee())
			{
				_enemyStateMachine.ChangeState(new EnemyMeleeAttackState(_enemyStateMachine));
			}
		}

		public override void ExitState()
		{
			ResetAnimationSpeed();
		}
		#region Helper Methods
		private bool ShouldNotFlee()
		{
			return Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
							< _enemyStateMachine.AttackRange && _enemyStateMachine.EnemyType == EnemyType.MeleeRanged;
		}

		private bool ShouldFlee()
		{
			return Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
							< _enemyStateMachine.FleeingRange && _enemyStateMachine.EnemyType == EnemyType.Ranged;
		}

		private bool OutOfShootingRange()
		{
			return Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
							> _enemyStateMachine.RangedAttackRange;
		}

		#endregion
	}
}