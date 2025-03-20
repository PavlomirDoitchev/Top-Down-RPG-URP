using Assets.Scripts.State_Machine.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerBasicAttackChainTwo : PlayerBaseState
    {
        private bool rotationLocked = false;
        private int attackIndex = 0;
        public PlayerBasicAttackChainTwo(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            if (meleeWeapon == null)
                Debug.LogError("No weapon!");
        }
        public override void EnterState()
        {
            _playerStateMachine.Animator.speed = _playerStateMachine.CharacterLevelDataSO[PlayerStats.Instance.CurrentLevel()].CharactAttackSpeed;
            _playerStateMachine.Animator.Play("2Hand-Sword-Attack2");
            SetWeaponDamage(attackIndex);
        }
        public override void UpdateState(float deltaTime)
        {
            Move(deltaTime);
            if (_playerStateMachine.InputManager.IsAttacking
                && _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .6f)
            {
                meleeWeapon.gameObject.SetActive(false);
                _playerStateMachine.ChangeState(new PlayerBasicAttackChainThree(_playerStateMachine));
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