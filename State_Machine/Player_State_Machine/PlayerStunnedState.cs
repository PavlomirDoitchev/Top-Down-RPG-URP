using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerStunnedState : PlayerBaseState
    {
        float _stunDuration = 2f;
        public PlayerStunnedState(PlayerStateMachine stateMachine, float stunDuration) : base(stateMachine)
        {
            _stunDuration = stunDuration;
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

