using Assets.Scripts.Player.Inventory;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Create New Weapon")]
public class WeaponDataSO : ScriptableObject, IPlayerItems
{
    public string itemId;
    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }
    
    [field: SerializeField] public string ItemName { get; set; }
    [field: SerializeField] public int ItemPrice { get; set; }
    public IPlayerItems.ItemType itemType;
    public WeaponType weaponType;
    public IPlayerItems.ItemRarity rarity;
    public string weaponName;
    public int minDamage;
    public int maxDamage;
    public float attackSpeed;
    public float criticalChance;

    [field: SerializeField] public bool IsStackable { get; set; } = false;
    [field: SerializeField] public bool IsEquippable { get; set; } = true;
    public enum WeaponType
    {
        Sword,
        Axe,
        Warhammer,
        Spear,
        Dagger,
        Bow,
        Crossbow,
        Scythe,
        Staff,
        Shield
    }
}
