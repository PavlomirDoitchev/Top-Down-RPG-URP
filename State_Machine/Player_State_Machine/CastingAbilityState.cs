using Assets.Scripts.Player;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
	public class CastingAbilityState : PlayerBaseState
	{
		private readonly Skills _skill;
		float timer;
		public CastingAbilityState(PlayerStateMachine stateMachine, Skills skill) : base(stateMachine)
		{
			_skill = skill;
		}
		public override void EnterState()
		{
			base.EnterState();
			ResetAnimationSpeed();
			_playerStateMachine.Animator.Play(_skill.castingAnimationName);
			timer = _skill.CastTime;
			//Debug.Log($"Entering CastingAbilityState with skill: {_skill.animationName}");
		}
		public override void UpdateState(float deltaTime)
		{
			PlayerMove(deltaTime);
			RotateToMouse();

			if (!_skill.AllowMovementWhileCasting	
				&& _playerStateMachine.CharacterController.velocity != Vector3.zero) 
			{
				//Debug.Log("Cannot move while casting!");
				_playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
				return;
			}



			if (!_skill.IsChanneled)
			{
				timer -= deltaTime;
				//Debug.Log($"Casting...: {timer}");
				if (timer <= 0)
				{
					_skill.UseSkill();
					_playerStateMachine.ChangeState(new ExecuteAbilityState(_playerStateMachine, _skill));
					return;
				}
			}
			else
			{
				_playerStateMachine.ChangeState(new ExecuteAbilityState(_playerStateMachine, _skill));
			}
		}
		public override void ExitState()
		{
			ResetAnimationSpeed();
		}
	}
}
