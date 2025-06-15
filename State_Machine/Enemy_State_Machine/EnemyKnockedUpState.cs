using UnityEngine;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
	public class EnemyKnockedUpState : EnemyBaseState
	{
		//private float knockUpDuration;
		//private float elapsedTime;
		Vector3 momentum;
		public EnemyKnockedUpState(EnemyStateMachine stateMachine) : base(stateMachine)
		{
			//knockUpDuration = duration;
		}
		public override void EnterState()
		{
			base.EnterState();	
			_enemyStateMachine.Agent.enabled = false;
			_enemyStateMachine.Animator.Play(_enemyStateMachine.KnockedUpAnimationName);
			_enemyStateMachine.IsKnockedUp = true;
			momentum = _enemyStateMachine.CharacterController.velocity;
			momentum.y = 0;
		}
		public override void UpdateState(float deltaTime)
		{
			//elapsedTime += deltaTime;
			//CheckForGlobalTransitions();

			Move(deltaTime);

			if (_enemyStateMachine.CharacterController.isGrounded)
			{
				_enemyStateMachine.Animator.Play(_enemyStateMachine.FallAnimationName);
				_enemyStateMachine.Agent.Warp(_enemyStateMachine.transform.position);
				_enemyStateMachine.Agent.enabled = true;


				if (_enemyStateMachine.PreviousState is EnemyPatrolState
					|| _enemyStateMachine.PreviousState is EnemyWanderState
					|| _enemyStateMachine.PreviousState is EnemyIdleState)

				{
					switch (_enemyStateMachine.EnemyType)
					{
						case EnemyType.Melee:
							_enemyStateMachine.ChangeState(new EnemyMeleeAttackState(_enemyStateMachine));
							break;
						case EnemyType.Ranged:
							_enemyStateMachine.ChangeState(new EnemyRangedAttackState(_enemyStateMachine));
							break;
						default:
							_enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine));
							break;
					}
				}
				else
				{
					_enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine));
				}
			}
		}
		public override void ExitState()
		{
			_enemyStateMachine.ForceReceiver.ResetForces();
			_enemyStateMachine.IsKnockedUp = false;
			ResetAnimationSpeed();
		}
	}
}
