using Assets.Scripts.Player;
using Assets.Scripts.State_Machine;
using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InputManager : MonoBehaviour
{
	[Header("Key Bindings")]
	[SerializeField] private KeyCode _jumpKey = KeyCode.Space;
	[SerializeField] private KeyCode _dodgeKey = KeyCode.LeftShift;
	[SerializeField] private KeyCode _attackKey = KeyCode.Mouse0;
	[SerializeField] private KeyCode _ability_One_Key = KeyCode.Mouse1;
	[SerializeField] private KeyCode _ability_Two_Key = KeyCode.Q;
	[SerializeField] private KeyCode _ability_Three_Key = KeyCode.E;
	[SerializeField] private KeyCode _ability_Four_Key = KeyCode.F;
	[SerializeField] private KeyCode _moveLeftKey = KeyCode.A;
	[SerializeField] private KeyCode _moveRightKey = KeyCode.D;
	[SerializeField] private KeyCode _moveUpKey = KeyCode.W;
	[SerializeField] private KeyCode _moveDownKey = KeyCode.S;
	PlayerManager _playerManager;
	private Dictionary<string, KeyCode> keyBindings;
	[field: SerializeField] public bool IsAttacking { get; set; }
	[field: SerializeField] public bool IsUsingAbility_Q { get; set; } = false;
	public Vector2 MoveInput { get; private set; }
	private void Awake()
	{
		keyBindings = new Dictionary<string, KeyCode>
		{
			{ "Jump", _jumpKey },
			{ "Attack", _attackKey },
			{ "Dodge", _dodgeKey },
			{ "AbilityOne", _ability_One_Key},
			{ "AbilityTwo", _ability_Two_Key },
			{ "AbilityThree", _ability_Three_Key },
			{ "AbilityFour", _ability_Four_Key },
			{ "MoveLeft", _moveLeftKey },
			{ "MoveRight", _moveRightKey },
			{ "MoveUp", _moveUpKey },
			{ "MoveDown", _moveDownKey }
		};
	}
	private void Start()
	{
		_playerManager = PlayerManager.Instance;
	}
	
	public bool PlayerJumpInput()
	{
		return Input.GetKey(keyBindings["Jump"]);
	}
	public bool PlayerDodgeInput()
	{
		return Input.GetKey(keyBindings["Dodge"]);
	}
	public bool AbilityOneInput() { return Input.GetKeyDown(keyBindings["AbilityOne"]); }
	public bool AbilityTwoInput() { return Input.GetKeyDown(keyBindings["AbilityTwo"]); }
	private void AttackInput()
	{
		if (Input.GetKey(keyBindings["Attack"]))
		{
			IsAttacking = true;
		}
		else
			IsAttacking = false;
	}
	#region Movement Input
	///// <summary>
	///// Move The Player.
	///// </summary>
	///// <param name="deltaTime"></param>
	//private void PlayerMove(float deltaTime)
	//{
	//	Vector3 movement = CalculateMovement();
	//	float speedModifier = _playerManager.PlayerStateMachine.PlayerStats.BaseMovementSpeed
	//		* (1 - _playerManager.PlayerStateMachine.PlayerStats.TotalSlowAmount);
	//	Mathf.Clamp(_playerManager.PlayerStateMachine.PlayerStats.TotalSlowAmount, 0, 0.95f);
	//	Move(movement * speedModifier, deltaTime);
	//	if (movement != Vector3.zero)
	//	{
	//		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement),
	//			_playerManager.PlayerStateMachine.PlayerStats.RotationSpeed * deltaTime);
	//		_playerManager.PlayerStateMachine.Animator.SetFloat("LocomotionSpeed", 1, .01f, deltaTime);
	//	}
	//	_playerManager.PlayerStateMachine.Animator.SetFloat("LocomotionSpeed", 0, .1f, deltaTime);

	//}
	///// <summary>
	///// Preserve Momentum
	///// </summary>
	///// <param name="movement"></param>
	///// <param name="deltaTime"></param>
	//private void Move(Vector3 movement, float deltaTime)
	//{
	//	_playerManager.PlayerStateMachine.CharacterController.Move((movement + _playerManager.PlayerStateMachine.ForceReceiver.Movement) * deltaTime);
	//}
	///// <summary>
	///// Preserve Momentum with no movement input.
	///// </summary>
	///// <param name="deltaTime"></param>
	//private void Move(float deltaTime)
	//{
	//	Move(Vector3.zero, deltaTime);
	//}
	//private Vector3 CalculateMovement()
	//{
	//	Vector2 input = MovementInput();
	//	Vector3 moveDir = new Vector3(input.x, 0, input.y).normalized;
	//	return moveDir;
	//}
	#endregion
	public Vector2 MovementInput()
	{
		float horizontal = GetAxis("Horizontal");
		float vertical = GetAxis("Vertical");
		MoveInput = new Vector2(horizontal, vertical).normalized;
		return MoveInput;
	}
	private float GetAxis(string axis)
	{
		float value = 0;
		if (axis == "Horizontal")
		{
			if (Input.GetKey(keyBindings["MoveLeft"])) value -= 1;
			if (Input.GetKey(keyBindings["MoveRight"])) value += 1;
		}
		else if (axis == "Vertical")
		{
			if (Input.GetKey(keyBindings["MoveDown"])) value -= 1;
			if (Input.GetKey(keyBindings["MoveUp"])) value += 1;
		}
		return value;
	}
	public KeyCode GetKey(string action)
	{
		return keyBindings.ContainsKey(action) ? keyBindings[action] : KeyCode.None;
	}
	public void RebindKey(string action, KeyCode newKey)
	{
		if (keyBindings.ContainsKey(action))
		{
			keyBindings[action] = newKey;
			PlayerPrefs.SetInt(action, (int)newKey);
			PlayerPrefs.Save();
		}
	}
	private KeyCode LoadKey(string action, KeyCode defaultKey)
	{
		return (KeyCode)PlayerPrefs.GetInt(action, (int)defaultKey);
	}
}

