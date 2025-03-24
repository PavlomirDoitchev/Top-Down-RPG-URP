using UnityEngine;
using Assets.Scripts.State_Machine;
using Assets.Scripts.Player;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    /// <summary>
    /// Initialize the Statemachine. Retrieve the equipped collider and the Player Stats.
    /// All other states derive from here. Methods written here can be applied to all states. 
    /// Consider adding references and methods here.
    /// </summary>
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine _playerStateMachine;
        protected MeleeWeapon meleeWeapon;
        //protected PlayerStats playerStats;
        protected readonly int activeLayer = 7;
        protected readonly int inactiveLayer = 3;
        private Quaternion lockedRotation;
        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this._playerStateMachine = stateMachine;
        }
        public override void EnterState()
        {
            base.EnterState();
            InitializeWeapon();
            SetWeaponActive(false);
            meleeWeapon.ClearHitEnemies();
        }
        protected void SetWeaponActive(bool isActive)
        {
            meleeWeapon.gameObject.layer = isActive ? activeLayer : inactiveLayer;
        }
        protected void InitializeWeapon()
        {
            if (_playerStateMachine.EquippedWeapon != null)
            {
                meleeWeapon = _playerStateMachine.EquippedWeapon.GetComponentInChildren<MeleeWeapon>();
            }

            if (meleeWeapon == null)
            {
                Debug.LogError("No weapon equipped in state: " + this.GetType().Name);
            }
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
        /// Preserve momentum. Used in PlayerMove().
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="deltaTime"></param>
        protected void Move(Vector3 movement, float deltaTime)
        {
            _playerStateMachine.CharacterController.Move((movement + _playerStateMachine.ForceReceiver.Movement) * deltaTime);
        }
        /// <summary>
        /// Apply physics. Sets Vector3 to 0.
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
                    _playerStateMachine.CharacterLevelDataSO[PlayerManager.Instance.playerStateMachine._PlayerStats.CurrentLevel()].
                    CharacterBaseRotationSpeed * 10 * deltaTime);
            }
        }
        /// <summary>
        /// Player Movement Logic
        /// </summary>
        /// <param name="deltaTime"></param>
        protected void PlayerMove(float deltaTime)
        {
            Vector3 movement = CalculateMovement();

            Move(movement * _playerStateMachine.CharacterLevelDataSO[_playerStateMachine._PlayerStats.CurrentLevel()].CharacterBaseMovementSpeed, deltaTime);

            if (movement != Vector3.zero)
            {
                _playerStateMachine.transform.rotation = Quaternion.Slerp(_playerStateMachine.transform.rotation,
                    Quaternion.LookRotation(movement), _playerStateMachine.CharacterLevelDataSO[PlayerManager.Instance.playerStateMachine._PlayerStats.CurrentLevel()]
                    .CharacterBaseRotationSpeed * deltaTime);
                _playerStateMachine.Animator.SetFloat("LocomotionSpeed", 1, .01f, deltaTime);
            }
            _playerStateMachine.Animator.SetFloat("LocomotionSpeed", 0, .1f, deltaTime);
        }

        private Vector3 CalculateMovement()
        {

            //Vector3 forward = _playerStateMachine.MainCameraTransform.forward;
            //Vector3 right = _playerStateMachine.MainCameraTransform.right;

            //forward.y = 0;
            //right.y = 0;

            //forward.Normalize();
            //right.Normalize();

            Vector2 moveInput = _playerStateMachine.InputManager.MoveInput;
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            //Vector3 moveDirection = forward moveInput.y + right moveInput.x;
            return moveDirection.normalized;
        }

        protected float CalculateDamage(int index)
        {
            float rollForCrit = Random.Range(0f, 1f);
            if (_playerStateMachine.CharacterLevelDataSO[PlayerManager.Instance.playerStateMachine._PlayerStats.CurrentLevel()].CharacterCriticalChance >= rollForCrit)
            {
                //Debug.Log("Critical!");
                return 1 +
                    (_playerStateMachine.CharacterLevelDataSO[PlayerManager.Instance.playerStateMachine._PlayerStats.CurrentLevel()].Strength *
                    _playerStateMachine.AbilityDataSO[index].damageMultiplier *
                    _playerStateMachine.CharacterLevelDataSO[PlayerManager.Instance.playerStateMachine._PlayerStats.CurrentLevel()].CharacterCriticalModifier
                    );
            }
            else
            {
                return 1 +
                   (_playerStateMachine.CharacterLevelDataSO[PlayerManager.Instance.playerStateMachine._PlayerStats.CurrentLevel()].Strength *
                   _playerStateMachine.AbilityDataSO[index].damageMultiplier
                   );
            }
        }
        /// <summary>
        /// Use the desired index of the AbilityDataSO
        /// </summary>
        /// <param name="index"></param>
        protected void SetWeaponDamage(int index)
        {
            float strengthMultiplier = CalculateDamage(index);
            meleeWeapon.MeleeWeaponDamage
                 (Random.Range(_playerStateMachine.EquippedWeaponDataSO.minDamage, _playerStateMachine.EquippedWeaponDataSO.maxDamage + 1),
                 strengthMultiplier,
                 Mathf.RoundToInt(_playerStateMachine.AbilityDataSO[index].damageMultiplier));
            //Debug.Log(_playerStateMachine.AttackData[index].attackName);
        }
    }
}
