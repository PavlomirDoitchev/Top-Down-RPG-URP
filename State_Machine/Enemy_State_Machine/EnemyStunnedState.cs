namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyStunnedState : EnemyBaseState
    {
        public EnemyStunnedState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.StunnedAnimationName, .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            if (CheckForGlobalTransitions()) return;
        }
        public override void ExitState()
        {

        }
    }
}
