using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
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
			int layerMask = LayerMask.GetMask("Ground", "Enemy");

			if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
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
		/// <summary>
		/// Instant rotation to the mouse position. 
		/// </summary>
		protected void RotateToMouse()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			int layerMask = LayerMask.GetMask("Ground", "Enemy", "Default");
			if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
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
		protected void PlayerAttackMove(float deltaTime)
		{
			Vector3 movement = CalculateMovement();
			float speedModifier = _playerStateMachine.PlayerStats.BaseMovementSpeed * (1 - _playerStateMachine.PlayerStats.TotalSlowAmount);
			Mathf.Clamp(_playerStateMachine.PlayerStats.TotalSlowAmount, 0f, 0.95f);
			Move(movement * speedModifier, deltaTime);

			if (movement != Vector3.zero)
			{
				_playerStateMachine.transform.rotation = Quaternion.Slerp(_playerStateMachine.transform.rotation,
					Quaternion.LookRotation(movement), _playerStateMachine.PlayerStats.RotationSpeed * deltaTime);
				_playerStateMachine.Animator.SetFloat("AttackLocomotionSpeed", .5f, .01f, deltaTime);
			}
			_playerStateMachine.Animator.SetFloat("AttackLocomotionSpeed", 0, .1f, deltaTime);
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
	}
}
