using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Save_Manager;

namespace Assets.Scripts.Player.Inventory
{
    public class PlayerInventory : MonoBehaviour//, ISaveManager
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
        //public void LoadData(GameData _data)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public void SaveData(ref GameData _data)
        //{
        //    _data.inventory.Clear();
        //    foreach (var item in inventoryItems)
        //    {
                
        //    }
        //}
    }
}

