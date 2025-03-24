using Assets.Scripts.Player;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class FighterAbilityQState : PlayerBaseState
    {
        private bool rotationLocked = false;
        private int attackIndex = 2;
        private Vector3 force;
        public FighterAbilityQState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            //if (meleeWeapon == null)
            //    Debug.LogError("No weapon!");
        }

        public override void EnterState()
        {
            base.EnterState();
            SetWeaponActive(false);
            //PlayerStats.Instance.UseResource(15);
            _playerStateMachine.Animator.speed = _playerStateMachine.CharacterLevelDataSO[_playerStateMachine._PlayerStats.CurrentLevel()].CharactAttackSpeed;
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
                SetWeaponActive(true);
                //meleeWeapon.gameObject.SetActive(true);
                rotationLocked = true;
                SetCurrentRotation();
            }

            if (_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                //meleeWeapon.gameObject.SetActive(false);
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