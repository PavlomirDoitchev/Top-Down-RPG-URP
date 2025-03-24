using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class FighterBasicAttackChainThree : PlayerBaseState
    {
        private bool rotationLocked = false;
        private int attackIndex = 1;
        public FighterBasicAttackChainThree(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            
        }
        public override void EnterState()
        {
            base.EnterState();
            _playerStateMachine.Animator.speed = _playerStateMachine.CharacterLevelDataSO[_playerStateMachine._PlayerStats.CurrentLevel()].CharactAttackSpeed;
            _playerStateMachine.Animator.Play("2Hand-Sword-Attack3");
            SetWeaponDamage(attackIndex);
        }
        public override void UpdateState(float deltaTime)
        {
            Move(deltaTime);
            if (_playerStateMachine.InputManager.IsAttacking
                && _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .6f)
            {
                SetWeaponActive(false);
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
                SetWeaponActive(true);
                rotationLocked = true;
                SetCurrentRotation();
            }

            if (_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                SetWeaponActive(false);
                _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
            }
        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
        }
    }
}