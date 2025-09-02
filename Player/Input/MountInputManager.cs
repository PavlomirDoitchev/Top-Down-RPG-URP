using System.Collections.Generic;
using UnityEngine;


public class MountInputManager : MonoBehaviour
{
    [Header("Key Bindings")]
    [SerializeField] private List<KeyBinding> keyBindingsList = new List<KeyBinding>();

    private Dictionary<string, KeyCode[]> keyBindingsDict;
    public Vector2 MoveInput { get; private set; }

    private void Awake()
    {
        keyBindingsDict = new Dictionary<string, KeyCode[]>();
        foreach (var binding in keyBindingsList)
            keyBindingsDict[binding.ActionName] = binding.Keys;

        LoadKeyBindings();
    }
    public Vector2 MovementInput()
    {
        float horizontal = 0;
        float vertical = 0;

        // if (Input.GetKey(keyBindingsDict["MoveLeft"][0])) horizontal -= 1;
        // if (Input.GetKey(keyBindingsDict["MoveRight"][0])) horizontal += 1;
        if (Input.GetKey(keyBindingsDict["MoveDown"][0])) vertical -= 1;
        if (Input.GetKey(keyBindingsDict["MoveUp"][0])) vertical += 1;

        MoveInput = new Vector2(horizontal, vertical).normalized;
        return MoveInput;
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

