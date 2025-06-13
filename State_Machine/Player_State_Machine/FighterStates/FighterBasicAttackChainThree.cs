using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class FighterBasicAttackChainThree : PlayerBaseState
    {
        private bool rotationLocked = false;
        public FighterBasicAttackChainThree(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            
        }
        public override void EnterState()
        {
            base.EnterState();
            int rank = _playerStateMachine.BasicAttackRank;
           SetAttackSpeed();
            _playerStateMachine.Animator.Play("2Hand-Sword-Attack3");
            //SetMeleeDamage(rank, AbilityType.BasicAttack, PlayerStatType.Strength);
        }
        public override void UpdateState(float deltaTime)
        {
            Move(deltaTime);
            if (_playerStateMachine.InputManager.IsAttacking
                && _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .6f)
            {
                _playerStateMachine.ForceReceiver.AddForce(-_playerStateMachine.transform.forward * _playerStateMachine.BasicAttackData[_playerStateMachine.BasicAttackRank].force);
                _playerStateMachine.ChangeState(new FighterBasicAttackChainOne(_playerStateMachine));
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

            if (_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
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