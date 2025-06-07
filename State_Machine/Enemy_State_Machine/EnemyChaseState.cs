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
                //if (_enemyStateMachine._enemyStateTypes == EnemyStateTypes.Patrol)
                //{
                //    _enemyStateMachine.ChangeState(new ReturnToOriginState(_enemyStateMachine));
                //    return;
                //}
                //else
                //{
                    _enemyStateMachine.ChangeState(new ReturnToOriginState(_enemyStateMachine));
                    return;
                //}
            }


            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) > _enemyStateMachine.ChaseDistance
                && !_enemyStateMachine.IsEnraged)
            {
                _enemyStateMachine.ChangeState(new EnemySuspicionState(_enemyStateMachine));

            }
            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) < _enemyStateMachine.AttackDistance)
            {
                _enemyStateMachine.ChangeState(new EnemyAttackState(_enemyStateMachine));
            }
        }

        public override void ExitState()
        {
            ResetMovementSpeed();
        }
    }
}
