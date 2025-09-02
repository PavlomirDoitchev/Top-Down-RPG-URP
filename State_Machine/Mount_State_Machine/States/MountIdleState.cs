namespace Assets.Scripts.State_Machine.Mount_State_Machine
{
    public class MountIdleState : MountBaseState
    {
        public MountIdleState(MountStateMachine stateMachine) : base(stateMachine) { }
        public override void EnterState()
        {
            base.EnterState();  
        }
        public override void UpdateState(float deltaTime)
        {
            stateMachine.SetSpeed(0f, deltaTime);
        }

        public override void ExitState() { }
    }

   
}
