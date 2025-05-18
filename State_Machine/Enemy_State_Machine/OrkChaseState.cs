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
            _enemyStateMachine.Agent.isStopped = false;
            _enemyStateMachine.Agent.speed = 3.5f;
            _enemyStateMachine.Animator.CrossFadeInFixedTime("running", .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            _enemyStateMachine.Agent.SetDestination(PlayerManager.Instance.PlayerStateMachine.transform.position);
            if(Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) > _enemyStateMachine.ChaseDistance)
            {
                _enemyStateMachine.ChangeState(new OrkIdleState(_enemyStateMachine));
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
