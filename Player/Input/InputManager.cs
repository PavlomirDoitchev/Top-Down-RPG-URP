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
	[SerializeField] private KeyCode _ability_Five_Key = KeyCode.R;
    [SerializeField] private KeyCode _moveLeftKey = KeyCode.A;
	[SerializeField] private KeyCode _moveRightKey = KeyCode.D;
	[SerializeField] private KeyCode _moveUpKey = KeyCode.W;
	[SerializeField] private KeyCode _moveDownKey = KeyCode.S;
	//PlayerManager _playerManager;
	private Dictionary<string, KeyCode> keyBindings;
	[field: SerializeField] public bool IsAttacking { get; set; }
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
            { "AbilityFive", _ability_Five_Key },
            { "MoveLeft", _moveLeftKey },
			{ "MoveRight", _moveRightKey },
			{ "MoveUp", _moveUpKey },
			{ "MoveDown", _moveDownKey }
		};
	}
	//private void Start()
	//{
	//	_playerManager = PlayerManager.Instance;
	//}
	
	public bool PlayerJumpInput() => Input.GetKey(keyBindings["Jump"]);
	public bool PlayerDodgeInput() => Input.GetKey(keyBindings["Dodge"]);
	public bool BasicAttackInput() => Input.GetKey(keyBindings["Attack"]); 
	public bool AbilityOneInput() => Input.GetKeyDown(keyBindings["AbilityOne"]); 
	public bool AbilityTwoInput() => Input.GetKeyDown(keyBindings["AbilityTwo"]);
	public bool AbilityThreeInput() => Input.GetKeyDown(keyBindings["AbilityThree"]);
	public bool AbilityFourInput() => Input.GetKeyDown(keyBindings["AbilityFour"]);
    public bool AbilityFiveInput() => Input.GetKeyDown(keyBindings["AbilityFive"]);
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

