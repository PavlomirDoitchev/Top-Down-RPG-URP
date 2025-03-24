using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerFallState : PlayerBaseState
    {
        private Vector3 momentum;
        private float maxFallSpeed;
        private int fallDamage;
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("2Hand-Sword-Fall", 0.1f);
            momentum = _playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
            maxFallSpeed = 0f;
        }

        public override void UpdateState(float deltaTime)
        {
            Move(momentum, deltaTime);

            if (_playerStateMachine.CharacterController.velocity.y < maxFallSpeed)
                maxFallSpeed = _playerStateMachine.CharacterController.velocity.y;


            if (_playerStateMachine.CharacterController.isGrounded)
            {
                if (maxFallSpeed < -20f)
                {
                    fallDamage = Mathf.RoundToInt(Mathf.Abs((maxFallSpeed * _playerStateMachine._PlayerStats.GetMaxHealth()) * 0.02f));
                    _playerStateMachine._PlayerStats.PlayerTakeDamage(fallDamage);
                }
                if (_playerStateMachine._PlayerStats.GetCurrentHealth() > 0)
                {
                    //_playerStateMachine.Animator.Play("2Hand-Sword-Land");
                    _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
                }
            }
        }
        public override void ExitState()
        {
            //Debug.Log("FallState Exit");
        }

    }
}
