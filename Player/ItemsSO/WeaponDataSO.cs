using Assets.Scripts.Player.Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Create New Weapon")]
public class WeaponDataSO : ScriptableObject, IPlayerItems
{

    public enum WeaponType
    {
        Sword,
        Bow,
        Staff,
        Dagger,
        Axe,
        Warhammer,
        Spear,
        Crossbow,
        Scythe,
        Halberd,
        Shield
    }
    public IPlayerItems.ItemType itemType;
    public WeaponType weaponType;
    public IPlayerItems.ItemRarity rarity;
    public string weaponName;
    public int minDamage;
    public int maxDamage;
    public float attackSpeed;
    public float criticalChance;
    public int price;

    [field: SerializeField] public bool IsStackable { get; set; } = false;
    [field: SerializeField] public bool IsEquippable { get; set; } = true;

    public void ItemName(string name)
    {
        weaponName = name;
    }

    public void ItemPrice(int cost)
    {
        price = cost;
    }

    public void SetItemRarity(IPlayerItems.ItemRarity itemRarity)
    {
        rarity = itemRarity;
    }

    public void SetItemType(IPlayerItems.ItemType type)
    {
        itemType = type;
    }
}