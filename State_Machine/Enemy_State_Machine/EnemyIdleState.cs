namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }
        float timer = 0f;
        float resetTimer = 0.1f; // Reset timer to avoid immediate state changes
        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.IdleAnimationName, .1f);
            switch (_enemyStateMachine.EnemyStateTypes)
            {
                case EnemyStateTypes.Patrol:
                    _enemyStateMachine.ChangeState(new EnemyPatrolState(_enemyStateMachine));
                    break;
                case EnemyStateTypes.Wander:
                    _enemyStateMachine.ChangeState(new EnemyWanderState(_enemyStateMachine));
                    break;
            }
        }

        public override void UpdateState(float deltaTime)
        {
            timer += deltaTime;
            if (CheckForGlobalTransitions()) return;
            if(timer < resetTimer) return; // Wait for the reset timer before processing further

            if (CanSeePlayer(_enemyStateMachine.AggroRange) || _enemyStateMachine.CheckForFriendlyInCombat)
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
            }
        }

        public override void ExitState()
        {
        }
    }
}