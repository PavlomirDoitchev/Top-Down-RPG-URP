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
            _enemyStateMachine.Agent.speed = _enemyStateMachine.WalkingSpeed;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.WalkAnimationName, .1f);
            _enemyStateMachine.Agent.SetDestination(_enemyStateMachine.OriginalPosition);
            BecomeUntargtable();
        }

        public override void UpdateState(float deltaTime)
        {
            if (_enemyStateMachine.Agent.remainingDistance <= _enemyStateMachine.MaxDistanceFromOrigin * 0.25f)
            {
                _enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine));
            }
        }

        public override void ExitState()
        {
            ResetAnimationSpeed();
            ResetMovementSpeed();
            BecomeTargetable();
        }
    }
}