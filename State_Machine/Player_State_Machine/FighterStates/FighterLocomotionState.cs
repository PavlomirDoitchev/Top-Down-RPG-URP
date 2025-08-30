using UnityEngine;
using Assets.Scripts.State_Machine;
using System;
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Player_State_Machine.FighterStates;
using System.Security.Cryptography;
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
			DoBasicAttack();
			DoAbilityOne();
			DoAbilityTwo();
			CastLightningShield();
            CastProjectile();

			//Test Mage skills
            //CastRainOfFire();
            CastCenterPull();
        }
		public override void ExitState()
		{
			ResetAnimationSpeed();
		}


		#region Helper Methods
		
		private void DoBasicAttack()
		{
			if (_playerStateMachine.InputManager.BasicAttackInput()
							&& SkillManager.Instance.FighterBasicAttack.CanUseSkill())
			{
                //_playerStateMachine.PlayerStats.UseResource(SkillManager.Instance.FighterBasicAttack.GetSkillCost());
                _playerStateMachine.ChangeState(new FighterBasicAttackChainOne(_playerStateMachine));
			}
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
		private void CastProjectile() 
		{
			if (_playerStateMachine.InputManager.AbilityFourInput()
				&& SkillManager.Instance.ProjectileAbility[0].CanUseSkill()) 
			{
				_playerStateMachine.ChangeState(new CastingAbilityState(_playerStateMachine, SkillManager.Instance.ProjectileAbility[0]));
			}
		}
		private void CastLightningShield()
        {
            if (_playerStateMachine.InputManager.AbilityThreeInput()
                && SkillManager.Instance.LightningShieldAbility.CanUseSkill())
            {
                _playerStateMachine.ChangeState(new CastingAbilityState(_playerStateMachine, SkillManager.Instance.LightningShieldAbility));
            }
        }
		private void CastCenterPull() 
		{
			if(_playerStateMachine.InputManager.AbilityFiveInput()
                && _playerStateMachine.Ability_Three_Rank > 0
                && SkillManager.Instance.CenterPullAbility.CanUseSkill())
            {
                _playerStateMachine.ChangeState(new CastingAbilityState(_playerStateMachine, SkillManager.Instance.CenterPullAbility));
            }
        }
        private void CastRainOfFire()
		{
			if (_playerStateMachine.InputManager.AbilityFiveInput()
				&& SkillManager.Instance.RainOfFireAbility.CanUseChanneledSkill()
				&& _playerStateMachine.Ability_Three_Data.Rank > 0) 
			{
                _playerStateMachine.ChangeState(new CastingAbilityState(_playerStateMachine, SkillManager.Instance.RainOfFireAbility));
			}
		}
		private void DoAbilityTwo()
		{
			if (_playerStateMachine.InputManager.AbilityTwoInput()
							&& _playerStateMachine.Ability_One_Rank > 0
							&& SkillManager.Instance.ShockwaveAbility.CanUseSkill())
			{
				//_playerStateMachine.PlayerStats.UseResource(SkillManager.Instance.ShockwaveAbility.GetSkillCost());
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
		#endregion
	

	}
}
