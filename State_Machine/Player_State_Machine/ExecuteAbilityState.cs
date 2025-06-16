
using Assets.Scripts.Player;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
	public class ExecuteAbilityState : PlayerBaseState
	{
		private readonly Skills _skill;
		public ExecuteAbilityState(PlayerStateMachine stateMachine, Skills skill) : base(stateMachine)
		{
			_skill = skill;
		}

		public override void EnterState()
		{
			base.EnterState();
			_playerStateMachine.Animator.Play(_skill.animationName);
		}

		public override void UpdateState(float deltaTime)
		{
			
		}

		public override void ExitState()
		{
			ResetAnimationSpeed();
		}
	}
}
