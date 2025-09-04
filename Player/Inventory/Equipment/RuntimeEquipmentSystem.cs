using UnityEngine;
using NHance.Assets.Scripts;
using NHance.Assets.Scripts.Items;
using NHance.Assets.Scripts.Enums;
using System.Collections.Generic;

/// <summary>
/// Runtime equipment manager for NHAvatar
/// Supports dynamic inventory, equipping, and unequipping.
/// </summary>
public class RuntimeEquipmentSystem : MonoBehaviour
{
    [Header("References")]
    public NHAvatar character; // Your NHAvatar
    [Header("Inventory")]
    public List<NHItem> inventory = new List<NHItem>(); // All available items

    private Dictionary<ItemTypeEnum, NHItem> equippedItems = new Dictionary<ItemTypeEnum, NHItem>();

    private void Start()
    {
        if (character == null)
        {
            Debug.LogError("NHAvatar reference is missing!");
            return;
        }

   
        //character.ClearItems();
        character.Compile();
        equippedItems.Clear();
    }

    private void Update()
    {
        HandleHotkeys();
    }

    /// <summary>
    /// Example hotkey system for testing
    /// </summary>
    private void HandleHotkeys()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ToggleItem(inventory[i]);
            }
        }

        // Example: clear all equipment
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ClearAllEquipment();
        }
    }

    /// <summary>
    /// Equip or unequip an item
    /// </summary>
    public void ToggleItem(NHItem item)
    {
        if (item == null) return;

        if (equippedItems.ContainsKey(item.Type))
        {
            // Unequip if already equipped
            UnequipItem(item.Type);
        }
        else
        {
            EquipItem(item);
        }
    }

    /// <summary>
    /// Equip an item and hide body parts
    /// </summary>
    public void EquipItem(NHItem item)
    {
        if (item == null) return;

        // Remove conflicting items (like two-handed weapons)
        if (item.IsTakeTwoHands)
        {
            if (equippedItems.ContainsKey(ItemTypeEnum.WeaponL)) UnequipItem(ItemTypeEnum.WeaponL);
            if (equippedItems.ContainsKey(ItemTypeEnum.WeaponR)) UnequipItem(ItemTypeEnum.WeaponR);
        }

        character.SetItem(item);

        // Hide covered body parts
        foreach (var part in item.GetPartByType())
        {
            foreach (var bodyPart in character.PartsMap)
            {
                if (bodyPart.Type == part && bodyPart.Target != null)
                    character.ActivateBodyPart(bodyPart.Target, false);
            }
        }

        character.Compile();
        equippedItems[item.Type] = item;
        Debug.Log($"Equipped {item.name}");
    }

    /// <summary>
    /// Unequip an item and show body parts
    /// </summary>
    public void UnequipItem(ItemTypeEnum type)
    {
        if (!equippedItems.ContainsKey(type)) return;

        var item = equippedItems[type];

        // Show body parts
        foreach (var part in item.GetPartByType())
        {
            foreach (var bodyPart in character.PartsMap)
            {
                if (bodyPart.Type == part && bodyPart.Target != null)
                    character.ActivateBodyPart(bodyPart.Target, true);
            }
        }

        character.ClearItems(type);
        character.Compile();
        equippedItems.Remove(type);
        Debug.Log($"Unequipped {type}");
    }

    /// <summary>
    /// Clears all equipped items
    /// </summary>
    public void ClearAllEquipment()
    {
        foreach (var type in new List<ItemTypeEnum>(equippedItems.Keys))
        {
            UnequipItem(type);
        }
    }
}
