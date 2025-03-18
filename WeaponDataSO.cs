using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Stats/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    public string weaponName;
    public int baseDamage;
    public float attackSpeed;
    public float criticalChance;
}