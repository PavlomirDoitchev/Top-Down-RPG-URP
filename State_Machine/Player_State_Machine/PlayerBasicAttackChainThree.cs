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
            if (meleeWeapon == null)
                Debug.LogError("No weapon!");
        }
        public override void EnterState()
        {
            _playerStateMachine.Animator.speed = _playerStateMachine.CharacterLevelDataSO[playerStats.CurrentLevel()].CharactAttackSpeed;
            _playerStateMachine.Animator.Play("2Hand-Sword-Attack3");
            SetWeaponDamage(attackIndex);
        }
        public override void UpdateState(float deltaTime)
        {
            Move(deltaTime);
            if (_playerStateMachine.InputManager.IsAttacking
                && _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .6f)
            {
                meleeWeapon.gameObject.SetActive(false);
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
                meleeWeapon.gameObject.SetActive(true);
                rotationLocked = true;
                SetCurrentRotation();
            }

            if (_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                meleeWeapon.gameObject.SetActive(false);
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
            }
        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
        }
    }
}