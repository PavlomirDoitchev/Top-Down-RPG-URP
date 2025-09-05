namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyLandState : EnemyBaseState
    {
        public EnemyLandState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
            ResetAnimationSpeed();
            _enemyStateMachine.IsFlying = false;
            _enemyStateMachine.Agent.isStopped = false;
        }

        public override void UpdateState(float deltaTime)
        {
           
        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
        }
    }

}

