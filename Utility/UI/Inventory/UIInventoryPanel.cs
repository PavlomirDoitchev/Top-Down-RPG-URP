using UnityEngine;
using UnityEngine.UI;
using NHance.Assets.Scripts.Items;
using System.Collections.Generic;

public class UIInventoryPanel : MonoBehaviour
{
    [Header("References")]
    public RuntimeEquipmentSystem equipmentSystem; // Reference to your runtime equipment manager
    public Transform buttonContainer;               // Inventory panel / grid layout parent
    public GameObject inventoryButtonPrefab;        // Prefab for each inventory button

    private void Start()
    {
        if (equipmentSystem == null || buttonContainer == null || inventoryButtonPrefab == null)
        {
            Debug.LogError("UIInventoryPanel references are not set!");
            return;
        }

        PopulateInventory();
    }

    /// <summary>
    /// Creates buttons for all items in the inventory
    /// </summary>
    private void PopulateInventory()
    {
        foreach (var item in equipmentSystem.inventory)
        {
            GameObject buttonGO = Instantiate(inventoryButtonPrefab, buttonContainer);
            Button button = buttonGO.GetComponent<Button>();
            Image icon = buttonGO.GetComponentInChildren<Image>();

            // Set icon sprite if available
            if (icon != null && item.GetComponent<SpriteRenderer>() != null)
            {
                icon.sprite = item.GetComponent<SpriteRenderer>().sprite;
            }

            // Add click listener
            button.onClick.AddListener(() => equipmentSystem.ToggleItem(item));
        }
    }
}
