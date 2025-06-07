using Assets.Scripts.Player;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class OrkChaseState : EnemyBaseState
    {

        public OrkChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = false;
            _enemyStateMachine.Agent.speed = _enemyStateMachine.RunningSpeed;
            _enemyStateMachine.Animator.CrossFadeInFixedTime("running", .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            if(PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentHealth() <= 0)
            {
                _enemyStateMachine.ChangeState(new OrkIdleState(_enemyStateMachine));
                return;
            }
            if (Vector3.Distance(_enemyStateMachine.OriginalPosition, _enemyStateMachine.transform.position) > _enemyStateMachine.MaxDistanceFromOrigin) 
            {
                if (_enemyStateMachine._enemyStateTypes == EnemyStateTypes.Patrol)
                {
                    _enemyStateMachine.ChangeState(new OrkPatrolState(_enemyStateMachine));
                    return;
                }
                else
                {
                    _enemyStateMachine.ChangeState(new ReturnToOriginState(_enemyStateMachine));
                    return;
                }
            }

            _enemyStateMachine.Agent.SetDestination(PlayerManager.Instance.PlayerStateMachine.transform.position);

            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) > _enemyStateMachine.ChaseDistance)
            {   
                _enemyStateMachine.ChangeState(new OrkSuspicionState(_enemyStateMachine));

            }
            if(Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) < _enemyStateMachine.AttackDistance)
            {
                _enemyStateMachine.ChangeState(new OrkAttackState(_enemyStateMachine));
            }
        }

        public override void ExitState()
        {
            
        }
    }
}
