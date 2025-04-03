using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    internal class PlayerStunnedState : PlayerBaseState
    {
        public PlayerStunnedState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Unarmed-Stunned", 0.1f);
        }

        public override void UpdateState(float deltaTime)
        {
            Move(deltaTime);
            Debug.Log("Stunned State");
        }

        public override void ExitState()
        {
        }
    }

}

