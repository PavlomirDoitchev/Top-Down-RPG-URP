
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    internal class EnemyDeathState : EnemyBaseState
    {
        public EnemyDeathState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.CrossFadeInFixedTime("dying 2", .1f);
        }
        public override void ExitState()
        {
        }

        public override void UpdateState(float deltaTime)
        {
        }
    }
}
