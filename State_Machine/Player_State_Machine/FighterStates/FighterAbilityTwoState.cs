
using Assets.Scripts.Player;

namespace Assets.Scripts.State_Machine.Player_State_Machine.FighterStates
{
	public class FighterAbilityTwoState : PlayerBaseState
	{
		public FighterAbilityTwoState(PlayerStateMachine stateMachine) : base(stateMachine)
		{
		}

		public override void EnterState()
		{
			base.EnterState();
			SkillManager.Instance.ShockwaveAbility.UseSkill();
			_playerStateMachine.Animator.Play("ARPG_Halberd_Attack_Heavy2 1");
			SetAttackSpeed();
			//RotateToMouse();
		}

		public override void UpdateState(float deltaTime)
		{
			Move(deltaTime);
		}

		public override void ExitState()
		{
			ResetAnimationSpeed();
		}
	}
}
