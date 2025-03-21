using Assets.Scripts.State_Machine.Player;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerDeathState : PlayerBaseState
    {
        private Vector3 momentum;
        float timer = 1f;
        public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            //_playerStateMachine.Ragdoll.ToggleRagdoll(true);
            _playerStateMachine.Animator.Play("2Hand-Sword-Knockdown1");
            momentum = _playerStateMachine.CharacterController.velocity;

        }
        public override void UpdateState(float deltaTime)
        {
            Move(momentum, deltaTime);
            timer -= deltaTime;
            if (timer <= 0f)
                timer = 0f;
            momentum.y -= timer;
            if (_playerStateMachine.CharacterController.isGrounded)
            {
                momentum = new Vector3(0, 0, 0);
            }
        }

        public override void ExitState()
        {
        }

    }
}