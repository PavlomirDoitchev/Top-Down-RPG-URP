using System.Collections;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerBasicAttackChainThree : PlayerBaseState
    {
        private bool rotationLocked = false;

        private Quaternion lockedRotation;

        public PlayerBasicAttackChainThree(PlayerStateMachine stateMachine) : base(stateMachine)
        {

        }
        public override void EnterState()
        {
            _playerStateMachine.Animator.Play("2Hand-Sword-Attack3");
        }
        public override void UpdateState(float deltaTime)
        {
            
            if (!rotationLocked)
            {
                RotateToMouse(deltaTime);
            }
            else
            {
                _playerStateMachine.transform.rotation = lockedRotation;
            }
            if (!rotationLocked && _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
            {
                rotationLocked = true;
                lockedRotation = _playerStateMachine.transform.rotation;
            }
            Move(deltaTime);

            if (_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .9f)
            {
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
            }

        }
        public override void ExitState()
        {

        }
    }
}