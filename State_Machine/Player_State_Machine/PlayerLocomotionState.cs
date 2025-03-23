using UnityEngine;
using Assets.Scripts.State_Machine;
using System;
using Assets.Scripts.State_Machine.Player;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerLocomotionState : PlayerBaseState
    {

        public PlayerLocomotionState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _playerStateMachine.Animator.CrossFadeInFixedTime("Basic_Locomotion", .1f);
        }
        public override void UpdateState(float deltaTime)
        {
            if (_playerStateMachine.CharacterController.velocity.y <= -10)
            {
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
                return;
            }
            if (Input.GetKey(_playerStateMachine.InputManager.GetKey("Dash")))
            {
                if (_playerStateMachine.CharacterController.velocity.x != 0 || _playerStateMachine.CharacterController.velocity.z != 0)
                {
                    _playerStateMachine.ChangeState(new PlayerDashState(_playerStateMachine));
                }
            }
            if (Input.GetKey(_playerStateMachine.InputManager.GetKey("Jump")) && _playerStateMachine.CharacterController.isGrounded)
            {
                _playerStateMachine.ChangeState(new PlayerJumpState(_playerStateMachine));
            }
            if (_playerStateMachine.InputManager.IsAttacking)
            {
                _playerStateMachine.ChangeState(new PlayerBasicAttackChainOne(_playerStateMachine));
            }
            if (_playerStateMachine.InputManager.IsUsingAbilityOne)
            {
                if(PlayerStats.Instance.CanUseSkill(15))
                    _playerStateMachine.ChangeState(new PlayerAbilityOne(_playerStateMachine));
            }
            PlayerMove(deltaTime);
        }


        public override void ExitState()
        {

        }

    }
}
