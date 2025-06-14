using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using UnityEngine.AI;
using UnityEngine;	
public class EnemyFleeState : EnemyBaseState
{
	float fleeRange;
	float fleeTimer = 5f;
	float fleeRepathInterval = .4f; // sample new position every 1 second
	float repathTimer;

	public EnemyFleeState(EnemyStateMachine stateMachine) : base(stateMachine) { }

	public override void EnterState()
	{
		base.EnterState();
		_enemyStateMachine.Agent.isStopped = false;
		_enemyStateMachine.Agent.speed = _enemyStateMachine.RunningSpeed;
		_enemyStateMachine.Animator.Play(_enemyStateMachine.RunAnimationName);
		fleeRange = _enemyStateMachine.FleeingRange;
		repathTimer = 0f;
	}

	public override void UpdateState(float deltaTime)
	{
		if (CheckForGlobalTransitions()) return;
		fleeTimer -= deltaTime;
		repathTimer -= deltaTime;

		if (fleeTimer <= 0f)
		{
			_enemyStateMachine.ChangeState(_enemyStateMachine.PreviousState);
			return;
		}

		var playerPos = PlayerManager.Instance.PlayerStateMachine.transform.position;
		var enemyPos = _enemyStateMachine.transform.position;

		if (Vector3.Distance(enemyPos, playerPos) > _enemyStateMachine.FleeingRange * 2)
		{
			_enemyStateMachine.ChangeState(new EnemyRangedAttackState(_enemyStateMachine));
			return;
		}

		// Only re-calculate flee path if repathTimer has expired
		if (repathTimer <= 0f)
		{
			repathTimer = fleeRepathInterval;

			Vector3 directionAway = (enemyPos - playerPos).normalized;

			const int maxAttempts = 5;
			Vector3 fleeDestination = enemyPos;
			bool validPathFound = false;

			for (int i = 0; i < maxAttempts; i++)
			{
				float randomAngle = Random.Range(-120f, 120f);
				Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);
				Vector3 randomFleeDirection = rotation * directionAway;

				Vector3 tentativeDestination = enemyPos + randomFleeDirection * _enemyStateMachine.FleeingDistanceAmount;

				if (NavMesh.SamplePosition(tentativeDestination, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
				{
					tentativeDestination = hit.position;

					NavMeshPath path = new NavMeshPath();
					bool pathFound = _enemyStateMachine.Agent.CalculatePath(tentativeDestination, path);

					if (pathFound && path.status == NavMeshPathStatus.PathComplete)
					{
						fleeDestination = tentativeDestination;
						validPathFound = true;
						break;
					}
				}
			}

			if (validPathFound)
			{
				_enemyStateMachine.Agent.SetDestination(fleeDestination);
			}
		}
	}

	public override void ExitState()
	{
		fleeRange = _enemyStateMachine.FleeingRange;
	}
}
