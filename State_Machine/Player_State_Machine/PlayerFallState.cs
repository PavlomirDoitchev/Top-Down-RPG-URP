using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerFallState : PlayerBaseState
    {
        private Vector3 momentum;
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            
            _playerStateMachine.Animator.CrossFadeInFixedTime("2Hand-Sword-Fall", 0.1f);
            momentum = _playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
        }

        public override void UpdateState(float deltaTime)
        {
            Move(momentum, deltaTime);
            if (_playerStateMachine.CharacterController.isGrounded)
            {
                
               // _playerStateMachine.Animator.Play("2Hand-Sword-Land");
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
                //return;
            }

        }
        public override void ExitState()
        {
            //Debug.Log("FallState Exit");
        }

    }
}
