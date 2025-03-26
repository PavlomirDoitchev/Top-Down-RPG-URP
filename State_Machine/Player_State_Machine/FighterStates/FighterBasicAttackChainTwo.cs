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
            int rank = _playerStateMachine.BasicAbilityRank;
            _playerStateMachine.Animator.speed = _playerStateMachine._PlayerStats.AttackSpeed;
            _playerStateMachine.Animator.Play("2Hand-Sword-Attack2");
            SetMeleeDamage(rank, AbilityType.BasicAttack, PlayerStatType.Strength);
        }
        public override void UpdateState(float deltaTime)
        {
            Move(deltaTime);
            if (_playerStateMachine.InputManager.IsAttacking
                && _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .75f)
            {
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
                SetWeaponActive(true);
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