using Assets.Scripts.State_Machine.Enemy_State_Machine;
using UnityEngine;
public class EnemyKnockbackState : EnemyBaseState
{
	private float knockbackDuration;
	private float elapsedTime;
    public EnemyKnockbackState(EnemyStateMachine stateMachine, float duration) : base(stateMachine)
	{
		knockbackDuration = duration;
	}

	public override void EnterState()
	{
		base.EnterState();
		_enemyStateMachine.Agent.enabled = false;
		_enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.HitAnimationName, 0.1f);
		_enemyStateMachine.IsKnockedBack = true;
	}

	public override void UpdateState(float deltaTime)
	{
		// CheckForGlobalTransitions();
		elapsedTime += deltaTime;
		Move(deltaTime);
		
		if (elapsedTime >= knockbackDuration)
		{
			_enemyStateMachine.IsKnockedBack = false;
            _enemyStateMachine.Agent.Warp(_enemyStateMachine.transform.position);
			_enemyStateMachine.Agent.enabled = true;


			if (_enemyStateMachine.PreviousState is EnemyPatrolState
				|| _enemyStateMachine.PreviousState is EnemyWanderState
				|| _enemyStateMachine.PreviousState is EnemyIdleState)

			{
				if (_enemyStateMachine.EnemyType == EnemyType.Melee)
					_enemyStateMachine.ChangeState(new EnemyMeleeAttackState(_enemyStateMachine));
				else if (_enemyStateMachine.EnemyType == EnemyType.Ranged || _enemyStateMachine.EnemyType == EnemyType.MeleeRanged)
					_enemyStateMachine.ChangeState(new EnemyRangedAttackState(_enemyStateMachine));
				else
					_enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine));
			}
			else
				_enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine));
		}
	}

	public override void ExitState()
	{
		_enemyStateMachine.IsKnockedBack = false;
		MovementSpeedRunning();
	}
	
}
