using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerAbilityOne : PlayerBaseState
    {
        private bool rotationLocked = false;
        private Quaternion lockedRotation;
        private int attackIndex = 1;
        private Vector3 force;
        public PlayerAbilityOne(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            //meleeWeapon = stateMachine.EquippedWeaponObject.GetComponentInChildren<MeleeWeapon>();
            if (meleeWeapon == null)
                Debug.LogError("No weapon!");
        }

        public override void EnterState()
        {
            _playerStateMachine.Animator.speed = _playerStateMachine.EquippedWeapon.attackSpeed;
            _playerStateMachine.Animator.Play("2Hand-Sword-Attack8");
            SetWeaponDamage(attackIndex);
            force = _playerStateMachine.transform.forward * _playerStateMachine.AttackData[attackIndex].force;
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
                _playerStateMachine.transform.rotation = lockedRotation;
            }
            if (!rotationLocked && _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
            {
                _playerStateMachine.ForceReceiver.AddForce(force);
                meleeWeapon.gameObject.SetActive(true);
                rotationLocked = true;
                lockedRotation = _playerStateMachine.transform.rotation;
            }

            if (_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                meleeWeapon.gameObject.SetActive(false);
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
            }
        }

        public override void ExitState()
        {
            _playerStateMachine.Animator.speed = 1f;
        }

    }
}