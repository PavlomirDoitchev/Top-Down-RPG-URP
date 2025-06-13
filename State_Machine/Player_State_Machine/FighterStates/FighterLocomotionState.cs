using UnityEngine;
using Assets.Scripts.State_Machine;
using System;
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Player_State_Machine.FighterStates;
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
			_playerStateMachine.Animator.CrossFadeInFixedTime("Basic_Locomotion", .1f);
		}
		public override void UpdateState(float deltaTime)
		{
			Fall();
			PlayerMove(deltaTime);
			DoJump();
			DoDodge();

			if (_playerStateMachine.InputManager.BasicAttackInput()
				&& SkillManager.Instance.FighterBasicAttack.CanUseSkill())
			{
				_playerStateMachine.ChangeState(new FighterBasicAttackChainOne(_playerStateMachine));
			}

			DoAbilityOne();
			DoAbilityTwo();
		}

		private void Fall()
		{
			if (_playerStateMachine.CharacterController.velocity.y <= -10)
			{
				_playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
				return;
			}
		}

		private void DoJump()
		{
			if (_playerStateMachine.InputManager.PlayerJumpInput() && _playerStateMachine.CharacterController.isGrounded)
			{
				_playerStateMachine.ChangeState(new PlayerJumpState(_playerStateMachine));
			}
		}

		private void DoAbilityTwo()
		{
			if (_playerStateMachine.InputManager.AbilityTwoInput()
							&& _playerStateMachine.Ability_One_Rank > 0
							&& SkillManager.Instance.FighterAbilityTwo.CanUseSkill())
			{
				_playerStateMachine.ChangeState(new FighterAbilityTwoState(_playerStateMachine));
			}
		}

		private void DoAbilityOne()
		{
			if (_playerStateMachine.InputManager.AbilityOneInput()
							&& _playerStateMachine.Ability_One_Rank > 0
							&& SkillManager.Instance.FighterAbilityOne.CanUseSkill())
			{
				_playerStateMachine.ChangeState(new FighterAbilityOneState(_playerStateMachine));
			}
		}

		private void DoDodge()
		{
			if (_playerStateMachine.InputManager.PlayerDodgeInput()
							&& _playerStateMachine.CharacterController.isGrounded
							&& SkillManager.Instance.Dodge.CanUseSkill()
							&& (_playerStateMachine.CharacterController.velocity.x != 0 
							|| _playerStateMachine.CharacterController.velocity.z != 0))
			{
				_playerStateMachine.ChangeState(new FighterDodgeState(_playerStateMachine));
			}
		}

		public override void ExitState()
		{
			ResetAnimationSpeed();
		}

	}
}
