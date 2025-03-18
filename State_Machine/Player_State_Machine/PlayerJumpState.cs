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
            _playerStateMachine.Animator.CrossFadeInFixedTime("2Hand-Sword-Jump", .1f);
            _playerStateMachine.ForceReceiver.Jump(_playerStateMachine.CharacterStats.JumpForce);
            momentum = _playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
        }
        public override void UpdateState(float deltaTime)
        {
            PlayerMove(deltaTime);
            //Move(momentum, deltaTime);
            if (_playerStateMachine.CharacterController.velocity.y <= 0)
            {
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
                return;
            }
        }

        public override void ExitState()
        {
        }
        private void PlayerMove(float deltaTime)
        {
            Vector3 movement = CalculateMovement();

            Move(movement * _playerStateMachine.CharacterStats.BaseMovementSpeed, deltaTime);

            if (movement != Vector3.zero)
            {
                _playerStateMachine.transform.rotation = Quaternion.Slerp(_playerStateMachine.transform.rotation,
                    Quaternion.LookRotation(movement), _playerStateMachine.CharacterStats.BaseRotationSpeed * deltaTime);
            }
        }

        private Vector3 CalculateMovement()
        {
            Vector3 forward = _playerStateMachine.MainCameraTransform.forward;
            Vector3 right = _playerStateMachine.MainCameraTransform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector2 moveInput = _playerStateMachine.InputManager.MoveInput;
            Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;
            return moveDirection.normalized;

        }
    }
}
