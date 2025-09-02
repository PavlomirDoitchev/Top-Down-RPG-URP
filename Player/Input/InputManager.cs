#region oldCode
//using Assets.Scripts.Player;
//using Assets.Scripts.State_Machine;
//using Assets.Scripts.State_Machine.Player_State_Machine;
//using System.Collections.Generic;
//using UnityEngine;

//public class InputManager : MonoBehaviour
//{
//    [Header("Key Bindings")]
//    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
//    [SerializeField] private KeyCode _dodgeKey = KeyCode.LeftShift;
//    [SerializeField] private KeyCode _attackKey = KeyCode.Mouse0;
//    [SerializeField] private KeyCode _ability_One_Key = KeyCode.Mouse1;
//    [SerializeField] private KeyCode _ability_Two_Key = KeyCode.Q;
//    [SerializeField] private KeyCode _ability_Three_Key = KeyCode.E;
//    [SerializeField] private KeyCode _ability_Four_Key = KeyCode.F;
//    [SerializeField] private KeyCode _ability_Five_Key = KeyCode.R;
//    [SerializeField] private KeyCode _moveLeftKey = KeyCode.A;
//    [SerializeField] private KeyCode _moveRightKey = KeyCode.D;
//    [SerializeField] private KeyCode _moveUpKey = KeyCode.W;
//    [SerializeField] private KeyCode _moveDownKey = KeyCode.S;

//    [field: SerializeField] public bool IsAttacking { get; set; }
//    public Vector2 MoveInput { get; private set; }

//    private Dictionary<string, KeyCode[]> keyBindings; 

//    private void Awake()
//    {
//        keyBindings = new Dictionary<string, KeyCode[]>
//        {
//            { "Jump", new KeyCode[]{ _jumpKey } },
//            { "Attack", new KeyCode[]{ _attackKey } },
//            { "Dodge", new KeyCode[]{ _dodgeKey } },
//            { "AbilityOne", new KeyCode[]{ _ability_One_Key } },
//            { "AbilityTwo", new KeyCode[]{ _ability_Two_Key } },
//            { "AbilityThree", new KeyCode[]{ _ability_Three_Key } },
//            { "AbilityFour", new KeyCode[]{ _ability_Four_Key } },
//            { "AbilityFive", new KeyCode[]{ _ability_Five_Key } },
//            { "AbilitySix", new KeyCode[]{ KeyCode.LeftShift, KeyCode.Space } },
//            { "MoveLeft", new KeyCode[]{ _moveLeftKey } },
//            { "MoveRight", new KeyCode[]{ _moveRightKey } },
//            { "MoveUp", new KeyCode[]{ _moveUpKey } },
//            { "MoveDown", new KeyCode[]{ _moveDownKey } }
//        };
//    }

//    // ----- Public Input Methods -----
//    public bool PlayerJumpInput() => GetKey(keyBindings["Jump"][0]);
//    public bool PlayerDodgeInput() => GetKeyDown(keyBindings["Dodge"][0]);
//    public bool BasicAttackInput() => GetKey(keyBindings["Attack"][0]);

//    public bool AbilityOneInput() => GetKeyDown(keyBindings["AbilityOne"][0]);
//    public bool AbilityTwoInput() => GetKeyDown(keyBindings["AbilityTwo"][0]);
//    public bool AbilityThreeInput() => GetKeyDown(keyBindings["AbilityThree"][0]);
//    public bool AbilityFourInput() => GetKeyDown(keyBindings["AbilityFour"][0]);
//    public bool AbilityFiveInput() => GetKeyDown(keyBindings["AbilityFive"][0]);
//    public bool AbilitySixInput() => GetComboDown(keyBindings["AbilitySix"]);

//    public Vector2 MovementInput()
//    {
//        float horizontal = GetAxis("Horizontal");
//        float vertical = GetAxis("Vertical");
//        MoveInput = new Vector2(horizontal, vertical).normalized;
//        return MoveInput;
//    }

//    // ----- Helper Methods -----
//    private float GetAxis(string axis)
//    {
//        float value = 0;
//        if (axis == "Horizontal")
//        {
//            if (GetKey(keyBindings["MoveLeft"][0])) value -= 1;
//            if (GetKey(keyBindings["MoveRight"][0])) value += 1;
//        }
//        else if (axis == "Vertical")
//        {
//            if (GetKey(keyBindings["MoveDown"][0])) value -= 1;
//            if (GetKey(keyBindings["MoveUp"][0])) value += 1;
//        }
//        return value;
//    }

//    private bool GetKey(KeyCode key) => Input.GetKey(key);
//    private bool GetKeyDown(KeyCode key) => Input.GetKeyDown(key);

//    private bool GetComboDown(KeyCode[] keys)
//    {
//        if (keys.Length == 0) return false;
//        // Check all keys except last are held
//        for (int i = 0; i < keys.Length - 1; i++)
//            if (!Input.GetKey(keys[i])) return false;

//        // Last key must be pressed down this frame
//        return Input.GetKeyDown(keys[keys.Length - 1]);
//    }

//    public KeyCode[] GetKey(string action) => keyBindings.ContainsKey(action) ? keyBindings[action] : new KeyCode[0];

//    public void RebindKey(string action, KeyCode[] newKeys)
//    {
//        if (keyBindings.ContainsKey(action))
//        {
//            keyBindings[action] = newKeys;
//            for (int i = 0; i < newKeys.Length; i++)
//                PlayerPrefs.SetInt(action + i, (int)newKeys[i]);
//            PlayerPrefs.Save();
//        }
//    }
//}
#endregion

using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Key Bindings")]
    [SerializeField] private List<KeyBinding> keyBindingsList = new List<KeyBinding>();

    private Dictionary<string, KeyCode[]> keyBindingsDict;

    [field: SerializeField] public bool IsAttacking { get; set; }
    public Vector2 MoveInput { get; private set; }

    private void Awake()
    {
        keyBindingsDict = new Dictionary<string, KeyCode[]>();
        foreach (var binding in keyBindingsList)
            keyBindingsDict[binding.ActionName] = binding.Keys;

        LoadKeyBindings();
    }

    // ----- Movement Input -----
    public Vector2 MovementInput()
    {
        float horizontal = 0;
        float vertical = 0;

        if (Input.GetKey(keyBindingsDict["MoveLeft"][0])) horizontal -= 1;
        if (Input.GetKey(keyBindingsDict["MoveRight"][0])) horizontal += 1;
        if (Input.GetKey(keyBindingsDict["MoveDown"][0])) vertical -= 1;
        if (Input.GetKey(keyBindingsDict["MoveUp"][0])) vertical += 1;

        MoveInput = new Vector2(horizontal, vertical).normalized;
        return MoveInput;
    }

    // ----- Basic Actions -----
    public bool PlayerJumpInput() => Input.GetKey(keyBindingsDict["Jump"][0]);
    public bool PlayerDodgeInput() => Input.GetKeyDown(keyBindingsDict["Dodge"][0]);
    public bool BasicAttackInput() => Input.GetKey(keyBindingsDict["Attack"][0]);
    public bool PlayerMountInput() => Input.GetKeyDown(keyBindingsDict["Mount"][0]);
    public bool PlayerDismountInput() => Input.GetKeyDown(keyBindingsDict["Dismount"][0]);

    // ----- Abilities -----
    public bool AbilityOneInput() => Input.GetKeyDown(keyBindingsDict["AbilityOne"][0]);
    public bool AbilityTwoInput() => Input.GetKeyDown(keyBindingsDict["AbilityTwo"][0]);
    public bool AbilityThreeInput() => Input.GetKeyDown(keyBindingsDict["AbilityThree"][0]);
    public bool AbilityFourInput() => Input.GetKeyDown(keyBindingsDict["AbilityFour"][0]);
    public bool AbilityFiveInput() => Input.GetKeyDown(keyBindingsDict["AbilityFive"][0]);
    public bool AbilitySixInput() => GetComboDown("AbilitySix");

    // ----- Combo Helper -----
    private bool GetComboDown(string action)
    {
        if (!keyBindingsDict.ContainsKey(action)) return false;

        KeyCode[] keys = keyBindingsDict[action];
        if (keys.Length == 0) return false;

        for (int i = 0; i < keys.Length - 1; i++)
            if (!Input.GetKey(keys[i])) return false;

        // Last key triggers
        return Input.GetKeyDown(keys[keys.Length - 1]);
    }

    public void RebindKey(string action, KeyCode[] newKeys)
    {
        if (!keyBindingsDict.ContainsKey(action)) return;

        keyBindingsDict[action] = newKeys;

        for (int i = 0; i < newKeys.Length; i++)
            PlayerPrefs.SetInt(action + i, (int)newKeys[i]);
        PlayerPrefs.SetInt(action + "Length", newKeys.Length);
        PlayerPrefs.Save();
    }

    // ----- Load Saved KeyBindings -----
    private void LoadKeyBindings()
    {
        foreach (var binding in keyBindingsList)
        {
            int length = PlayerPrefs.GetInt(binding.ActionName + "Length", binding.Keys.Length);
            KeyCode[] keys = new KeyCode[length];

            for (int i = 0; i < length; i++)
                keys[i] = (KeyCode)PlayerPrefs.GetInt(binding.ActionName + i, (int)binding.Keys[i]);

            binding.Keys = keys;
            keyBindingsDict[binding.ActionName] = keys;
        }
    }
}