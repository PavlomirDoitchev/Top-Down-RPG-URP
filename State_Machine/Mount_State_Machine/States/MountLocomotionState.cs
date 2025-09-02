using Assets.Scripts.State_Machine.Player_State_Machine;

namespace Assets.Scripts.State_Machine.Mount_State_Machine.States
{
    public class MountLocomotionState : MountBaseState
    {
        public MountLocomotionState(MountStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
            stateMachine.Animator.CrossFadeInFixedTime("Mount_Locomotion", .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            MountMove(deltaTime);
            RotateMountToMouse(deltaTime);
        }
        public override void ExitState()
        {
        }
    }
}
