using Assets.Scripts.Player;
using Assets.Scripts.Enemies;
using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.IdleAnimationName, .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            
            if (CheckForGlobalTransitions()) return;
            
            if (_enemyStateMachine._enemyStateTypes == EnemyStateTypes.Patrol)
            {
                _enemyStateMachine.ChangeState(new EnemyPatrolState(_enemyStateMachine));
            }
            else if (_enemyStateMachine.PatrolPath == null && _enemyStateMachine._enemyStateTypes == EnemyStateTypes.Wander) 
            {
                _enemyStateMachine.ChangeState(new EnemyWanderState(_enemyStateMachine));
            }
            //if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) < _enemyStateMachine.AggroRange)
          
            if(CanSeePlayer(_enemyStateMachine.AggroRange))
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
            }
            
        }

        public override void ExitState()
        {
        }
    }
}