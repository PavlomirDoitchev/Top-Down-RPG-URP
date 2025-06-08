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

            switch (_enemyStateMachine._enemyStateTypes)
            {
                case EnemyStateTypes.Patrol:
                    _enemyStateMachine.ChangeState(new EnemyPatrolState(_enemyStateMachine));
                    break;
                case EnemyStateTypes.Wander:
                    _enemyStateMachine.ChangeState(new EnemyWanderState(_enemyStateMachine));
                    break;
            }

            if (CanSeePlayer(_enemyStateMachine.AggroRange))
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
            }
        }

        public override void ExitState()
        {
        }
    }
}