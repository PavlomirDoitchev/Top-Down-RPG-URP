using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerFallState : PlayerBaseState
    {
        private Vector3 momentum;
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            momentum = _playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
        }

        public override void UpdateState(float deltaTime)
        {
            Move(momentum, deltaTime);
            if (_playerStateMachine.CharacterController.isGrounded)
            {
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
                return;
            }

        }
        public override void ExitState()
        {
            Debug.Log("FallState Exit");
        }

    }
}
