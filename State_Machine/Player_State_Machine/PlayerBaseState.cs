using UnityEngine;
using Assets.Scripts.State_Machine;
using Assets.Scripts.Player;
using DamageNumbersPro;
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
        
        private Quaternion lockedRotation;
        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this._playerStateMachine = stateMachine;
        }
        public override void EnterState()
        {
            base.EnterState();
            //Debug.Log($"Entering state: {this.GetType().Name}");

            
        }

        /// <summary>
        /// Set animation speed back to normal playback. 
        /// Commonly used in Exit State after using an ability
        /// </summary>
        protected void ResetAnimationSpeed()
        {
            _playerStateMachine.Animator.speed = 1f;
        }
        protected void SetAttackSpeed() 
        {
            _playerStateMachine.Animator.speed = _playerStateMachine.PlayerStats.TotalAttackSpeed;
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
        /// Use to make the character rotate to the mouse position. Don't forget to set the terrain collider to the "Ground" layer for this to work.    
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
                    _playerStateMachine.PlayerStats.RotationSpeed * deltaTime);
            }
        }
        protected void RotateToMouse()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
			{
				Vector3 targetPoint = hit.point;
				targetPoint.y = _playerStateMachine.transform.position.y;
				Quaternion targetRotation = Quaternion.LookRotation(targetPoint - _playerStateMachine.transform.position);
				_playerStateMachine.transform.rotation = targetRotation;
			}
		}
		/// <summary>
		/// Player Movement Logic
		/// </summary>
		/// <param name="deltaTime"></param>
		protected void PlayerMove(float deltaTime)
        {
            Vector3 movement = CalculateMovement();
            float speedModifier = _playerStateMachine.PlayerStats.BaseMovementSpeed * (1 - _playerStateMachine.PlayerStats.TotalSlowAmount);
            Mathf.Clamp(_playerStateMachine.PlayerStats.TotalSlowAmount, 0f, 0.95f);
            Move(movement * speedModifier, deltaTime);

            if (movement != Vector3.zero)
            {
                _playerStateMachine.transform.rotation = Quaternion.Slerp(_playerStateMachine.transform.rotation,
                    Quaternion.LookRotation(movement), _playerStateMachine.PlayerStats.RotationSpeed * deltaTime);
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

            Vector2 moveInput = _playerStateMachine.InputManager.MovementInput();
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
        protected float GetStatValue(PlayerStatType statType)
        {
            switch (statType)
            {
                case PlayerStatType.Strength:
                    return _playerStateMachine.PlayerStats.Strength + _playerStateMachine.PlayerStats.TotalStatChangeStrength;
                case PlayerStatType.Dexterity:
                    return _playerStateMachine.PlayerStats.Dexterity + _playerStateMachine.PlayerStats.TotalStatChangeDexterity;
                case PlayerStatType.Intellect:
                    return _playerStateMachine.PlayerStats.Intellect + _playerStateMachine.PlayerStats.TotalStatChangeIntellect;
                default:
                    return 0;
            }
        }
        //public enum AbilityType
        //{
        //    BasicAttack,
        //    AbilityQ,
        //    AbilityE,
        //    AbilityR
        //}
        
        //private int GetAbilityRank(AbilityType abilityType)
        //{
        //    switch (abilityType)
        //    {
        //        case AbilityType.BasicAttack:
        //            return _playerStateMachine.BasicAttackRank;
        //        case AbilityType.AbilityQ:
        //            return _playerStateMachine.QAbilityRank;
        //        // Add more abilities as needed
        //        default:
        //            return 0;
        //    }
        //}
        //private bool CriticalStrikeSuccessfull()
        //{
        //    float rollForCrit = Random.Range(0f, 1f);
        //    if (_playerStateMachine.PlayerStats.CriticalChance >= rollForCrit)
        //    {
        //        //Debug.Log("Critical!");
        //        return true;
        //    }
        //    return false;
        //}
        //private float GetAbilityMultiplier(AbilityType abilityType, int rank)
        //{
        //    switch (abilityType)
        //    {
        //        case AbilityType.BasicAttack:
        //            return _playerStateMachine.basicAbilityData[rank].damageMultiplier;
        //        case AbilityType.AbilityQ:
        //            return _playerStateMachine.qAbilityData[rank].damageMultiplier;
        //        // Add cases for AbilityE and AbilityR 
        //        default:
        //            return 1f; 
        //    }
        //}
        //protected float CalculateMeleeDamage(AbilityType abilityType, PlayerStatType statType)
        //{
        //    int rank = GetAbilityRank(abilityType);
        //    float statValue = GetStatValue(statType);
        //    if(statValue < 0)
        //    {
        //        Debug.LogWarning($"Stat value for {statType} is negative: {statValue}. Using 0 instead.");
        //        statValue = 0;
        //    }
        //    float damageMultiplier = GetAbilityMultiplier(abilityType, rank);
        //    //use a percentage of your attack stat. In SetMeleeDamage 
        //    float baseDamage = 1 + (statValue *
        //                   damageMultiplier);
        //    _playerStateMachine.damageText.SetColor(Color.white);
        //    if (CriticalStrikeSuccessfull())
        //    {
        //        baseDamage *= _playerStateMachine.PlayerStats.CriticalModifier;
        //        _playerStateMachine.damageText.SetColor(Color.yellow);
        //    }
        //    return baseDamage;
        //}
        /// <summary>
        /// Choose the ability rank and the stat type to use as a modifier for the melee damage.
        /// </summary>
        /// <param name="abilityRank"></param>
        /// <param name="abilityType"></param>
        /// <param name="statType"></param>
        //protected void SetMeleeDamage(int abilityRank, AbilityType abilityType, PlayerStatType statType)
        //{
        //    float calculatedDamage = CalculateMeleeDamage(abilityType, statType);
        //    meleeWeapon.MeleeWeaponDamage
        //         (Random.Range(meleeWeapon.EquippedWeaponDataSO.minDamage, meleeWeapon.EquippedWeaponDataSO.maxDamage + 1),
        //         calculatedDamage,
        //         abilityRank);
        //}
     
    }
}
