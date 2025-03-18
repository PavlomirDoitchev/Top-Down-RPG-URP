using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Stats/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    public string weaponName;
    public int minDamage;
    public int maxDamage;
    public float attackSpeed;
    public float criticalChance;
}