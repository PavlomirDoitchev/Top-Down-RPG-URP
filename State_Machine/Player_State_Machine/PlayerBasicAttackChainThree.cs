using Assets.Scripts.State_Machine.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerBasicAttackChainThree : PlayerBaseState
    {
        private bool rotationLocked = false;
        private int attackIndex = 1;
        public PlayerBasicAttackChainThree(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            
        }
        public override void EnterState()
        {
            base.EnterState();
            _playerStateMachine.Animator.speed = _playerStateMachine.CharacterLevelDataSO[PlayerStats.Instance.CurrentLevel()].CharactAttackSpeed;
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
                _playerStateMachine.ChangeState(new PlayerBasicAttackChainOne(_playerStateMachine));
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
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
            }
        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
        }
    }
}