namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerMountedState : PlayerBaseState
    {
        private float currentSpeed;

        public PlayerMountedState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            base.EnterState();
            currentSpeed = 0f;

            // Player anim → mount up
            _playerStateMachine.Animator.CrossFadeInFixedTime("Mount", 0.1f);

            // TODO: trigger horse animator too (via reference or event)
        }

        public override void UpdateState(float deltaTime)
        {
            // Rotate horse slowly towards cursor
            RotateMountToMouse(deltaTime, 0.5f);

            // Mounted locomotion
            MountedMove(deltaTime, ref currentSpeed, acceleration: 3f, deceleration: 4f, maxSpeed: 6f);
        }

        public override void ExitState()
        {
            // Player anim → dismount
            //_playerStateMachine.Animator.CrossFadeInFixedTime("Mount_Exit", 0.1f);

            // TODO: trigger horse dismount anim
        }
    }
}
