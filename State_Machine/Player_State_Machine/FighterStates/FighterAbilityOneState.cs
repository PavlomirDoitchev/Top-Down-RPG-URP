using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class FighterAbilityOneState : PlayerBaseState
    {
        public FighterAbilityOneState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
			_playerStateMachine.Animator.Play("ARPG_Dual_Wield_Attack_Heavy1");
            
        }
        
        public override void UpdateState(float deltaTime)
        {
            PlayerMove(deltaTime);
        }

        public override void ExitState()
        {
            SkillManager.Instance.FighterAbilityOne.particle.Stop();
			ResetAnimationSpeed();
        }
       
    }
}