using UnityEngine;
using Assets.Scripts.State_Machine;
using System;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class TestState : PlayerBaseState
    {

        public TestState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void EnterState()
        {
            Debug.Log("EnterState Enter");
        }
        public override void UpdateState(float deltaTime)
        {
            if (Input.GetKeyDown(_playerStateMachine.InputManager.GetKey("Attack")))
            {
                Debug.Log("Player Attacked!");
            }

            PlayerMove(deltaTime);

        }


        public override void ExitState()
        {
            Debug.Log("TestState Exit");
        }
        private void PlayerMove(float deltaTime)
        {
            Vector3 movement = CalculateMovement();
            movement *= _playerStateMachine.BaseMovementSpeed * deltaTime;
            _playerStateMachine.CharacterController.Move(movement);

            if (movement != Vector3.zero)
            {
                _playerStateMachine.transform.rotation = Quaternion.Slerp(_playerStateMachine.transform.rotation,
                    Quaternion.LookRotation(movement), _playerStateMachine.BaseRotationSpeed * deltaTime);
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
