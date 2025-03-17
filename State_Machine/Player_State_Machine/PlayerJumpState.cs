using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerJumpState : PlayerBaseState
    {
        private Vector3 momentum;
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            _playerStateMachine.ForceReceiver.Jump(_playerStateMachine.JumpForce);
            momentum = _playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
        }
        public override void UpdateState(float deltaTime)
        {
            Move(momentum, deltaTime);
            if (_playerStateMachine.CharacterController.velocity.y <= 0)
            {
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
                return;
            }
        }

        public override void ExitState()
        {
        }

    }
}
