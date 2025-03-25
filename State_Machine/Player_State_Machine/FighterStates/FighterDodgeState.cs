using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class FighterDodgeState : PlayerBaseState
    {
        Vector3 dashDirection;
        Vector3 dashVelocity;
        //float dashSpeed = 10f;
        public FighterDodgeState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _playerStateMachine.Animator.speed = 1.2f;
            _playerStateMachine.Animator.Play("2Hand-Sword-DiveRoll-Forward1");
            Physics.IgnoreLayerCollision(10, _playerStateMachine.gameObject.layer, true);
            dashDirection = _playerStateMachine.transform.forward;
            dashVelocity = dashDirection * _playerStateMachine.CharacterLevelDataSO[_playerStateMachine._PlayerStats.CurrentLevel()].CharacterBaseMovementSpeed;
            dashVelocity.y = Physics.gravity.y;

        }


        public override void UpdateState(float deltaTime)
        {
            Move(dashVelocity, deltaTime);   
            if (_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f) 
            {
                _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
            }

        }
        public override void ExitState()
        {
            Physics.IgnoreLayerCollision(10, _playerStateMachine.gameObject.layer, false);
            ResetAnimationSpeed();
        }
    }
}
