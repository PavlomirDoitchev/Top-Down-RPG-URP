using Assets.Scripts.Player.Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Create New Weapon")]
public class WeaponDataSO : ScriptableObject, IItem
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
    public IItem.ItemType itemType;
    public WeaponType weaponType;
    public IItem.ItemRarity rarity;
    public string weaponName;
    public int minDamage;
    public int maxDamage;
    public float attackSpeed;
    public float criticalChance;
    public int price;


    public void ItemName(string name)
    {
        weaponName = name;
    }

    public void ItemPrice(int cost)
    {
        price = cost;
    }

    public void SetItemRarity(IItem.ItemRarity itemRarity)
    {
        rarity = itemRarity;
    }

    public void SetItemType(IItem.ItemType type)
    {
        itemType = type;
    }
}