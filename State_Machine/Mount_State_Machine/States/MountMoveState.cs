namespace Assets.Scripts.State_Machine.Mount_State_Machine.States
{
    public class MountMoveState : MountBaseState
    {
        public MountMoveState(MountStateMachine stateMachine) : base(stateMachine) { }
        public override void EnterState()
        {
            base.EnterState();
        }
        public override void UpdateState(float deltaTime)
        {
            stateMachine.SetSpeed(stateMachine.MaxSpeed, deltaTime);
        }

        public override void ExitState() { }
    }
}
