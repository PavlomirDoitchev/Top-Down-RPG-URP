using Assets.Scripts.State_Machine.Player;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerAbilityOne : PlayerBaseState
    {
        private bool rotationLocked = false;
        private int attackIndex = 2;
        private Vector3 force;
        public PlayerAbilityOne(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            if (meleeWeapon == null)
                Debug.LogError("No weapon!");
        }

        public override void EnterState()
        {
            _playerStateMachine.Animator.speed = _playerStateMachine.CharacterLevelDataSO[PlayerStats.Instance.CurrentLevel()].CharactAttackSpeed;
            _playerStateMachine.Animator.Play("2Hand-Sword-Attack8");
            SetWeaponDamage(attackIndex);
            force = _playerStateMachine.transform.forward * _playerStateMachine.AbilityDataSO[attackIndex].force;
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