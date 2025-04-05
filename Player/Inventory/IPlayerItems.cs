namespace Assets.Scripts.Player.Inventory
{
    public interface IPlayerItems
    {
        public enum ItemType
        {
            Weapon,
            Armor,
            Consumable,
            QuestItem,
            Miscellaneous
        }
        public enum ItemRarity
        {
            Common,
            Uncommon,
            Rare,
            Epic,
            Legendary
        }
        public bool IsStackable { get; set; }
        public bool IsEquippable { get; set; }
        public string ItemName { get; set; }
        public int ItemPrice { get; set; }  
    }
}
