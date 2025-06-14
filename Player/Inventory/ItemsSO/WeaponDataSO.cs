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
    [field: SerializeField] public GameObject weaponPrefab { get; set; }
	[field: SerializeField] public Sprite ItemIcon { get; set; }
    [field: SerializeField] public string ItemName { get; set; }
    [field: SerializeField] public int ItemPrice { get; set; }

    public IPlayerItems.ItemType itemType;
    public WeaponType weaponType;
    public IPlayerItems.ItemRarity rarity;

	public int minDamage;
    public int maxDamage;
	public int staminaModifier;
    public int strengthModifier;
    public int dexterityModifier;
	public int intelligenceModifier;
    public int wisdomModifier;


    public float attackSpeedModifier;
    [Range(0,1)] public float criticalChanceModifier;
    [Range(0,2)] public float criticalDamageModifier;
	public int resourceModifier;
    public float movementSpeedModifier;
    public int armorModifier;

	[field: SerializeField] public bool IsStackable { get; set; } = false;
    [field: SerializeField] public bool IsEquippable { get; set; } = true;
    public enum WeaponType
    {
        TwoHandedSword,
        TwoHandedAxe,
        Warhammer,
        Spear,
		OneHandedSword,
		Dagger,
        Bow,
        Staff,
        Shield
    }
}
