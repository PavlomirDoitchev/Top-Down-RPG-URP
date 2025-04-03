using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Player.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        public static PlayerInventory Instance;
        //public InventoryUI inventoryUI { get; private set; }
        public List<IPlayerItems> inventoryItems = new List<IPlayerItems>();
        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else
                Instance = this;
            //inventoryUI = FindFirstObjectByType<InventoryUI>();
        }

    }
}

