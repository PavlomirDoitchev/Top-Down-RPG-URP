using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class FighterBasicAttackChainTwo : PlayerBaseState
    {
        private bool rotationLocked = false;
        public FighterBasicAttackChainTwo(PlayerStateMachine stateMachine) : base(stateMachine)
        {
           
        }
        public override void EnterState()
        {
            base.EnterState();
            int rank = _playerStateMachine.BasicAttackRank;
            _playerStateMachine.Animator.speed = _playerStateMachine.PlayerStats.TotalAttackSpeed;
            _playerStateMachine.Animator.Play("2Hand-Sword-Attack2");
            _playerStateMachine.MainAttacks[1].Play();
            SetAttackSpeed();
        }
        public override void UpdateState(float deltaTime)
        {
            Move(deltaTime);
			if (_playerStateMachine.InputManager.BasicAttackInput()
				&& _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .75f)
			{
                _playerStateMachine.ForceReceiver.AddForce(-_playerStateMachine.transform.forward * _playerStateMachine.BasicAttackData[_playerStateMachine.BasicAttackRank].force);
                _playerStateMachine.ChangeState(new FighterBasicAttackChainThree(_playerStateMachine));
            }

            if (!rotationLocked)
            {
                RotateToMouse(deltaTime);
            }
            else
            {
                LockRotation();
            }
            if (!rotationLocked && _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
            {
                _playerStateMachine.ForceReceiver.AddForce(_playerStateMachine.transform.forward * _playerStateMachine.BasicAttackData[_playerStateMachine.BasicAttackRank].force);

                rotationLocked = true;
                SetCurrentRotation();
            }

            if (_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .85f)
            {
                _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
            }

        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
        }
    }
}