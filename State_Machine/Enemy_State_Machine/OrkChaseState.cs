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
            _enemyStateMachine.Agent.speed = _enemyStateMachine.Agent.speed;
            _enemyStateMachine.Animator.CrossFadeInFixedTime("running", .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            if(PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentHealth() <= 0)
            {
                return;
            }
            
            _enemyStateMachine.Agent.SetDestination(PlayerManager.Instance.PlayerStateMachine.transform.position);
            if(Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) > _enemyStateMachine.ChaseDistance)
            {   
                _enemyStateMachine.Agent.SetDestination(_enemyStateMachine.OriginalPosition);
                if (Vector3.Distance(_enemyStateMachine.OriginalPosition, _enemyStateMachine.transform.position) <= _enemyStateMachine.Agent.stoppingDistance) 
                {
                    _enemyStateMachine.ChangeState(new OrkIdleState(_enemyStateMachine));
                
                }
            }
            if(Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) < _enemyStateMachine.MeleeAttackDistance)
            {
                _enemyStateMachine.ChangeState(new OrkAttackState(_enemyStateMachine));
            }
        }

        public override void ExitState()
        {
            
        }
    }
}
