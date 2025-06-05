using Assets.Scripts.Player;
using Assets.Scripts.Enemies;
using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class OrkIdleState : EnemyBaseState
    {
        public OrkIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.CrossFadeInFixedTime("idle", .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            if(PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentHealth() <= 0)
            {
                return;
            }
            if (_enemyStateMachine.PatrolPath != null)
            {
                _enemyStateMachine.ChangeState(new OrkPatrolState(_enemyStateMachine));
            }
            else if (_enemyStateMachine.PatrolPath == null && _enemyStateMachine._enemyStateTypes == EnemyStateTypes.Wander) 
            {
                _enemyStateMachine.ChangeState(new OrkWanderState(_enemyStateMachine));
            }
            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) < _enemyStateMachine.AggroRange) 
            {
                _enemyStateMachine.ChangeState(new OrkChaseState(_enemyStateMachine));
            }
            
        }

        public override void ExitState()
        {
        }
    }
}