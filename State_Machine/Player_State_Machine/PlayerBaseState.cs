using UnityEngine;
using Assets.Scripts.State_Machine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    /// <summary>
    /// The first state of the state machine. 
    /// Consider adding references and methods here.
    /// </summary>
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine _playerStateMachine;
        protected MeleeWeapon meleeWeapon;
        private Quaternion lockedRotation;
        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this._playerStateMachine = stateMachine;
            meleeWeapon = stateMachine.EquippedWeaponCollider.GetComponentInChildren<MeleeWeapon>();
        }
        /// <summary>
        /// Set animation speed back to normal playback. 
        /// Commonly used in Exit State after using an ability
        /// </summary>
        protected void ResetAnimationSpeed()
        {
            _playerStateMachine.Animator.speed = 1f;
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
        protected void SetCurrentRotation() 
        {
            lockedRotation = _playerStateMachine.transform.rotation;
        }
        protected void LockRotation()
        {
            _playerStateMachine.transform.rotation = lockedRotation;
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
                    _playerStateMachine.CharacterStats.CharacterBaseRotationSpeed * 5 * deltaTime);
            }
        }
        protected void PlayerMove(float deltaTime)
        {
            Vector3 movement = CalculateMovement();

            Move(movement * _playerStateMachine.CharacterStats.CharacterBaseMovementSpeed, deltaTime);

            if (movement != Vector3.zero)
            {
                _playerStateMachine.transform.rotation = Quaternion.Slerp(_playerStateMachine.transform.rotation,
                    Quaternion.LookRotation(movement), _playerStateMachine.CharacterStats.CharacterBaseRotationSpeed * deltaTime);
                _playerStateMachine.Animator.SetFloat("LocomotionSpeed", 1, .01f, deltaTime);
            }
            _playerStateMachine.Animator.SetFloat("LocomotionSpeed", 0, .1f, deltaTime);
        }

        private Vector3 CalculateMovement()
        {
            Vector3 forward = _playerStateMachine.MainCameraTransform.forward;
            Vector3 right = _playerStateMachine.MainCameraTransform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector2 moveInput = _playerStateMachine.InputManager.MoveInput;
            Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;
            return moveDirection.normalized;

        }
        protected float CalculateDamage(int index)
        {
            return 1 + (_playerStateMachine.CharacterStats.Strength * _playerStateMachine.AttackData[index].damageMultiplier);
        }
        /// <summary>
        /// Use the desired index of the AttackDataSO
        /// </summary>
        /// <param name="index"></param>
        protected void SetWeaponDamage(int index)
        {
            float strengthMultiplier = CalculateDamage(index);
            meleeWeapon.MeleeWeaponDamage
                (Random.Range(_playerStateMachine.EquippedWeapon.minDamage, _playerStateMachine.EquippedWeapon.maxDamage + 1), 
                strengthMultiplier,
                Mathf.RoundToInt(_playerStateMachine.AttackData[index].damageMultiplier));
            //Debug.Log(_playerStateMachine.AttackData[index].attackName);
        }
    }
}
