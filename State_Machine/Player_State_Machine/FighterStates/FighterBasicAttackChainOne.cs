using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class FighterBasicAttackChainOne : PlayerBaseState
    {
        private bool rotationLocked = false;

        public FighterBasicAttackChainOne(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
            int rank = _playerStateMachine.BasicAbilityRank;
            _playerStateMachine.Animator.speed = _playerStateMachine._PlayerStats.AttackSpeed;
            _playerStateMachine.Animator.Play("2Hand-Sword-Attack1");

            SetMeleeDamage(rank, AbilityType.BasicAttack, PlayerStatType.Strength);
        }
        public override void UpdateState(float deltaTime)
        {
            int rank = _playerStateMachine.BasicAbilityRank;

            Move(deltaTime);
            if (_playerStateMachine.InputManager.IsAttacking
                && _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .75f)
            {
                _playerStateMachine.ChangeState(new FighterBasicAttackChainTwo(_playerStateMachine));
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
                //SpawnSlashEffect(rank);
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
        //private void SpawnSlashEffect(int abilityRank)
        //{
            
        //}
        //private void SpawnSlashEffect(int abilityRank)
        //{
        //    if (_playerStateMachine.basicAbilityData != null && _playerStateMachine.basicAbilityData[abilityRank] != null)
        //    {
        //        // Get the sword's position
        //        Transform swordTransform = meleeWeapon.transform; // Ensure this is set in PlayerStateMachine
        //        if (swordTransform == null) return;

        //        // Instantiate the effect at the sword's position
        //        ParticleSystem effect = GameObject.Instantiate(_playerStateMachine.basicAbilityData[abilityRank].VFX, swordTransform.position, Quaternion.identity, swordTransform);
        //        effect.Play(); // Play the effect immediately
        //        GameObject.Destroy(effect.gameObject, effect.main.duration); // Destroy after duration
        //    }
        //}
    }
}