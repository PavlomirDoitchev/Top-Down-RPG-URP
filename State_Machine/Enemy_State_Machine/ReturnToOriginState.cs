using UnityEngine;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class ReturnToOriginState : EnemyBaseState
    {
        public ReturnToOriginState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = false;
            MovementSpeedEnraged();
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.WalkAnimationName, .1f);
            _enemyStateMachine.Agent.SetDestination(_enemyStateMachine.OriginalPosition);
            BecomeUntargtable();
        }

        public override void UpdateState(float deltaTime)
        {
            if (_enemyStateMachine.Agent.remainingDistance <= _enemyStateMachine.MaxDistanceFromOrigin * 0.25f)
            {
                switch (_enemyStateMachine.EnemyStateTypes)
                {
                    case EnemyStateTypes.Patrol:
                        _enemyStateMachine.ChangeState(new EnemyPatrolState(_enemyStateMachine));
                        break;
                    case EnemyStateTypes.Idle:
                        _enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine));
                        break;
                    case EnemyStateTypes.Wander:
                        _enemyStateMachine.ChangeState(new EnemyWanderState(_enemyStateMachine));
                        break;
                }
            }
        }

        public override void ExitState()
        {
            ResetAnimationSpeed();
            MovementSpeedRunning();
            BecomeTargetable();
        }
    }
}