using Assets.Scripts.Player;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class FighterAbilityQState : PlayerBaseState
    {
        private bool rotationLocked = false;
        private Vector3 force;
        public FighterAbilityQState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            int rank = _playerStateMachine.QAbilityRank;
            _playerStateMachine.Animator.speed = _playerStateMachine._PlayerStats.AttackSpeed;
            _playerStateMachine.Animator.Play("2Hand-Sword-Attack8");
            SetMeleeDamage(rank, AbilityType.AbilityQ, PlayerStatType.Strength);
            force = _playerStateMachine.transform.forward * _playerStateMachine.qAbilityData[rank].force;
        }

        public override void UpdateState(float deltaTime)
        {
            Move(deltaTime);
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
                _playerStateMachine.ForceReceiver.AddForce(force);
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