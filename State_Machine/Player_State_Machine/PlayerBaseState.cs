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
                    _playerStateMachine._PlayerStats.RotationSpeed * deltaTime);
            }
        }
        /// <summary>
        /// Player Movement Logic
        /// </summary>
        /// <param name="deltaTime"></param>
        protected void PlayerMove(float deltaTime)
        {
            Vector3 movement = CalculateMovement();

            Move(movement * _playerStateMachine._PlayerStats.BaseMovementSpeed, deltaTime);

            if (movement != Vector3.zero)
            {
                _playerStateMachine.transform.rotation = Quaternion.Slerp(_playerStateMachine.transform.rotation,
                    Quaternion.LookRotation(movement), _playerStateMachine._PlayerStats.RotationSpeed * deltaTime);
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
        public enum PlayerStatType
        {
            Strength,
            Dexterity,
            Intellect
        }
        public enum AbilityType
        {
            BasicAttack,
            AbilityQ,
            AbilityE,
            AbilityR
        }
        private float GetStatValue(PlayerStatType statType)
        {
            switch (statType)
            {
                case PlayerStatType.Strength:
                    return _playerStateMachine._PlayerStats.Strength;
                case PlayerStatType.Dexterity:
                    return _playerStateMachine._PlayerStats.Dexterity;
                case PlayerStatType.Intellect:
                    return _playerStateMachine._PlayerStats.Intellect;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// Determine whether the attack should apply critical strike modifiers.
        /// </summary>
        /// <returns></returns>
        private bool CriticalStrikeSuccessfull() 
        {
            float rollForCrit = Random.Range(0f, 1f);
            if (_playerStateMachine._PlayerStats.CriticalChance >= rollForCrit) 
            {
                Debug.Log("Critical!");
                return true;
            }
            return false;
        }
        protected float CalculateMeleeDamage(AbilityType abilityType, PlayerStatType statType)
        {
            int rank = GetAbilityRank(abilityType);
            float statValue = GetStatValue(statType);
            float damageMultiplier = GetAbilityMultiplier(abilityType, rank);
            float baseDamage = 1 + (statValue *
                           damageMultiplier);
            if (CriticalStrikeSuccessfull())
            {
                baseDamage *= _playerStateMachine._PlayerStats.CriticalModifier;
            }
            return baseDamage;
        }
        private int GetAbilityRank(AbilityType abilityType)
        {
            switch (abilityType)
            {
                case AbilityType.BasicAttack:
                    return _playerStateMachine.BasicAbilityRank;
                case AbilityType.AbilityQ:
                    return _playerStateMachine.QAbilityRank;
                // Add more abilities as needed
                default:
                    return 0;
            }
        }
        private float GetAbilityMultiplier(AbilityType abilityType, int rank)
        {
            switch (abilityType)
            {
                case AbilityType.BasicAttack:
                    return _playerStateMachine.basicAbilityData[rank].damageMultiplier;
                case AbilityType.AbilityQ:
                    return _playerStateMachine.qAbilityData[rank].damageMultiplier;
                // Add cases for AbilityE and AbilityR 
                default:
                    return 1f; 
            }
        }
        private Basic_Ability_SO GetAbilityData(AbilityType abilityType)
        {
            switch (abilityType)
            {
                case AbilityType.BasicAttack:
                    return _playerStateMachine.basicAbilityData[_playerStateMachine.BasicAbilityRank];

                case AbilityType.AbilityQ:
                    return _playerStateMachine.qAbilityData[_playerStateMachine.QAbilityRank];

                

                default:
                    Debug.LogWarning($"AbilityType {abilityType} not found!");
                    return null;
            }
        }
        protected void SetMeleeDamage(int abilityRank, AbilityType abilityType, PlayerStatType statType)
        {
            float multiplier = CalculateMeleeDamage(abilityType, statType);
            //Debug.Log($"Using {statType} as a modifier");
            meleeWeapon.MeleeWeaponDamage
                 (Random.Range(meleeWeapon.EquippedWeaponDataSO.minDamage, meleeWeapon.EquippedWeaponDataSO.maxDamage + 1),
                 multiplier,
                 abilityRank);
            //ApplyAbilityEffects(abilityType);
        }
        //private void ApplyAbilityEffects(AbilityType abilityType)
        //{
        //    Basic_Ability_SO ability = GetAbilityData(abilityType);

        //    if (ability == null) return;

        //    foreach (var buff in ability.buffs)
        //    {
        //        _playerStateMachine._PlayerStats.ApplyBuff(new Buff(buff.Type, buff.Duration, buff.EffectStrength));
        //    }

        //    foreach (var debuff in ability.debuffs)
        //    {
        //        _playerStateMachine._PlayerStats.ApplyDebuff(new Debuff(debuff.debuffType, debuff.duration, debuff.effectStrength));
        //    }
        //}
    }
}
