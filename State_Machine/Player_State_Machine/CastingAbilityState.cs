using Assets.Scripts.Player;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
	public class CastingAbilityState : PlayerBaseState
	{
		private readonly Skills _skill;
		Vector3 characterVelocity;
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
			characterVelocity = _playerStateMachine.CharacterController.velocity;
			Debug.Log($"Entering CastingAbilityState with skill: {_skill.animationName}");
		}
		public override void UpdateState(float deltaTime)
		{
			PlayerMove(deltaTime);
			RotateToMouse();

			if (!_skill.AllowMovementWhileCasting && characterVelocity.x != 0f || characterVelocity.y !=0 && characterVelocity.z !=0) 
			{
				_playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
			}


			if (_skill.IsChanneled)
			{
				_skill.UseSkill();
				timer -= deltaTime;
				
				Debug.Log($"Channeled...: {timer}");
				if (timer <= 0)
				{
					_playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
					return;
				}
			}
			else if(!_skill.IsChanneled)
			{
				timer -= deltaTime;
				Debug.Log($"Casting...: {timer}");
				if (timer <= 0)
				{
					_skill.UseSkill();
					_playerStateMachine.ChangeState(new ExecuteAbilityState(_playerStateMachine, _skill));
					return;
				}
			}
		}
		public override void ExitState()
		{
			ResetAnimationSpeed();
		}
	}
}
