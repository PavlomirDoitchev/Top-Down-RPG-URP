
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyPlayerIsDeadState : EnemyBaseState
    {
        public EnemyPlayerIsDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            ResetAnimationSpeed();
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.IdleAnimationName, .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            // No updates needed when the player is dead
        }

        public override void ExitState()
        {
            // No exit logic needed when the player is dead
        }
    }
}
    

