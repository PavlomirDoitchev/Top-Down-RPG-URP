using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //public static InputManager Instance;
    [Header("Key Bindings")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _attackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _dodgeKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _ability_Q_Key = KeyCode.Alpha1;
    [SerializeField] private KeyCode _moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode _moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode _moveUpKey = KeyCode.W;
    [SerializeField] private KeyCode _moveDownKey = KeyCode.S;

    private Dictionary<string, KeyCode> keyBindings;
    public bool IsAttacking { get; private set; }
    public bool IsUsingAbility_Q { get; private set; }
    public Vector2 MoveInput { get; private set; }
    private void Awake()
    {
        keyBindings = new Dictionary<string, KeyCode>
        {
            { "Jump", _jumpKey },
            { "Attack", _attackKey },
            { "Dodge", _dodgeKey },
            { "AbilityQ", _ability_Q_Key},
            { "MoveLeft", _moveLeftKey },
            { "MoveRight", _moveRightKey },
            { "MoveUp", _moveUpKey },
            { "MoveDown", _moveDownKey }
        };
    }
    private void Update()
    {
        MovementInput();
        AttackInput();
        Ability_Q();
    }
    private void Ability_Q()
    {
        if (Input.GetKey(keyBindings["AbilityQ"]))
            IsUsingAbility_Q = true;
        else
            IsUsingAbility_Q = false;
    }
    private void AttackInput()
    {
        if (Input.GetKey(keyBindings["Attack"]))
            IsAttacking = true;
        else
            IsAttacking = false;
    }
    private void MovementInput()
    {
        float horizontal = GetAxis("Horizontal");
        float vertical = GetAxis("Vertical");
        MoveInput = new Vector2(horizontal, vertical);
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
