using UnityEngine;
using Assets.Scripts.State_Machine;
using System;
using Assets.Scripts.Player;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class FighterLocomotionState : PlayerBaseState
    {
        ParticleSystem particleTornado;
        public FighterLocomotionState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            ResetAnimationSpeed();
            particleTornado = SkillManager.Instance.fighterQ.particle;
            particleTornado.Stop();
            _playerStateMachine.Animator.CrossFadeInFixedTime("Basic_Locomotion", .1f);
        }
        public override void UpdateState(float deltaTime)
        {
            if (_playerStateMachine.CharacterController.velocity.y <= -10)
            {
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
                return;
            }
            
            if (Input.GetKey(_playerStateMachine.InputManager.GetKey("Dodge")))
            {
                if (_playerStateMachine.CharacterController.velocity.x != 0 || _playerStateMachine.CharacterController.velocity.z != 0)
                {
                    _playerStateMachine.ChangeState(new FighterDodgeState(_playerStateMachine));
                }
            }
            if (Input.GetKey(_playerStateMachine.InputManager.GetKey("Jump")) && _playerStateMachine.CharacterController.isGrounded)
            {
                _playerStateMachine.ChangeState(new PlayerJumpState(_playerStateMachine));
            }
            if (_playerStateMachine.InputManager.IsAttacking)
            {
                if (SkillManager.Instance.fighterBasicAttack.CanUseSkill()) { }
            }
            if (_playerStateMachine.InputManager.IsUsingAbility_Q && _playerStateMachine.QAbilityRank > 0)
            {
                if (SkillManager.Instance.fighterQ.CanUseSkill()) { }
            }
            PlayerMove(deltaTime);
        }


        public override void ExitState()
        {
            ResetAnimationSpeed();
        }

    }
}
