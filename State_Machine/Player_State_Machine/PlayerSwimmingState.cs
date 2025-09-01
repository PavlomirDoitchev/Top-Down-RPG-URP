using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerSwimmingState : PlayerBaseState
    {
        public PlayerSwimmingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Entering Swimming State");
            ResetAnimationSpeed();
            _playerStateMachine.Animator.CrossFadeInFixedTime("Swimming", .1f);
        }
        public override void UpdateState(float deltaTime)
        {
            PlayerSwim(deltaTime);
        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
        }

    }
}
