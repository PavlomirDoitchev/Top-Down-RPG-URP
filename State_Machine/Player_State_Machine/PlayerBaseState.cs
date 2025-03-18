using UnityEngine;
using Assets.Scripts.State_Machine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public abstract class PlayerBaseState : State
    {
         
        protected PlayerStateMachine _playerStateMachine;
        protected MeleeWeapon meleeWeapon;
        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this._playerStateMachine = stateMachine;
            meleeWeapon = stateMachine.EquippedWeaponObject.GetComponentInChildren<MeleeWeapon>();
        }
       
        /// <summary>
        /// Move with Input Reading.
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="deltaTime"></param>
        protected void Move(Vector3 movement, float deltaTime) 
        {
            _playerStateMachine.CharacterController.Move((movement + _playerStateMachine.ForceReceiver.Movement) * deltaTime);
        }
        /// <summary>
        /// Apply physics. Does not take in Input Reading.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected void Move(float deltaTime) 
        {
            Move(Vector3.zero, deltaTime);
        }
        /// <summary>
        /// Use to make the character rotate to the mouse position.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected void RotateToMouse(float deltaTime)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Vector3 targetPoint = hit.point;
                targetPoint.y = _playerStateMachine.transform.position.y;
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - _playerStateMachine.transform.position);
                _playerStateMachine.transform.rotation = Quaternion.Slerp(
                    _playerStateMachine.transform.rotation,
                    targetRotation,
                    _playerStateMachine.CharacterStats.BaseRotationSpeed * 5 * deltaTime);
            }
        }

        protected float CalculateDamage()
        {
            return 1 + (_playerStateMachine.CharacterStats.Strength * 0.1f);
        }
        protected void SetWeaponDamage()
        {
            float strengthMultiplier = CalculateDamage();
            meleeWeapon.SetDamage(_playerStateMachine.EquippedWeapon.baseDamage, strengthMultiplier);
        }
    }
}
