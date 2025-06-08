using Assets.Scripts.Player;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyChaseState : EnemyBaseState
    {

        public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = false;
            if (_enemyStateMachine.IsEnraged)
            {
                _enemyStateMachine.Agent.speed = _enemyStateMachine.EnragedSpeed;
            }
            else
            {
                _enemyStateMachine.Agent.speed = _enemyStateMachine.RunningSpeed;
            }
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.RunAnimationName, .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            if (CheckForGlobalTransitions()) return;
            _enemyStateMachine.Agent.SetDestination(PlayerManager.Instance.PlayerStateMachine.transform.position);

            if (Vector3.Distance(_enemyStateMachine.OriginalPosition, _enemyStateMachine.transform.position) > _enemyStateMachine.MaxDistanceFromOrigin
                && !_enemyStateMachine.IsEnraged)
            {
                _enemyStateMachine.ChangeState(new ReturnToOriginState(_enemyStateMachine));
                return;
            }

            if (!_enemyStateMachine.IsEnraged
                && (Vector3.Distance(
                PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) > _enemyStateMachine.ChaseDistance
                || !HasLineOfSight()))
            {
                _enemyStateMachine.ChangeState(new EnemySuspicionState(_enemyStateMachine));
            }

            // Check if the enemy can see the player and is a melee enemy and switch to the appropriate attack state
            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) < _enemyStateMachine.AttackDistanceToleranceBeforeChasing
                && HasLineOfSight() 
                && MeleeEnemy())
            {
                _enemyStateMachine.ChangeState(new EnemyMeleeAttackState(_enemyStateMachine));
            }
            //else
            //    _enemyStateMachine.ChangeState(new EnemyRangedAttackState(_enemyStateMachine));
        }

        public override void ExitState()
        {
            MovementSpeedRunning();
        }
    }
}
