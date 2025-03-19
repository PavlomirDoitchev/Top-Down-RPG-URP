using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Create New Weapon")]
public class WeaponDataSO : ScriptableObject
{
    public string weaponName;
    public int minDamage;
    public int maxDamage;
    public float attackSpeed;
    public float criticalChance;
}