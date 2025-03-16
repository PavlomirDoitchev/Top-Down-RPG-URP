using UnityEngine;
using Assets.Scripts.State_Machine;
using System;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerLocomotionState : PlayerBaseState
    {

        public PlayerLocomotionState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void EnterState()
        {
           _playerStateMachine.Animator.CrossFadeInFixedTime("Basic_Locomotion", .1f);
        }
        public override void UpdateState(float deltaTime)
        {
            if (_playerStateMachine.InputManager.IsAttacking)
            {
                _playerStateMachine.ChangeState(new PlayerAttackState(_playerStateMachine, 0));
                return;
            }
            if (_playerStateMachine.InputManager.IsUsingAbilityOne) 
            {
                _playerStateMachine.ChangeState(new PlayerAbilityOne(_playerStateMachine));
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
           
            Move(movement * _playerStateMachine.BaseMovementSpeed, deltaTime);

            if (movement != Vector3.zero)
            {
                _playerStateMachine.transform.rotation = Quaternion.Slerp(_playerStateMachine.transform.rotation,
                    Quaternion.LookRotation(movement), _playerStateMachine.BaseRotationSpeed * deltaTime);
                _playerStateMachine.Animator.SetFloat("LocomotionSpeed", 1, .01f, deltaTime);
            }
            _playerStateMachine.Animator.SetFloat("LocomotionSpeed", 0, .1f, deltaTime);
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
