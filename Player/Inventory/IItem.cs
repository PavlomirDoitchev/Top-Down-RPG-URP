namespace Assets.Scripts.Player.Inventory
{
    public interface IItem
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
        public void ItemName(string name);  
        public void SetItemType(ItemType type);    
        public void SetItemRarity(ItemRarity itemRarity);
        public void ItemPrice(int cost);
    }
}
