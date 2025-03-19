using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerDeathState : PlayerBaseState
    {
        public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            _playerStateMachine.Animator.Play("2Hand-Sword-Knockdown1");

        }
        public override void UpdateState(float deltaTime)
        {
            Move(deltaTime);
        }

        public override void ExitState()
        {
        }

    }
}