using UnityEngine;

[CreateAssetMenu(fileName = "New Character Level", menuName = "New Character Level/Create New Character Level")]
public class CharacterLevelSO : ScriptableObject
{
    public string Class;
    public int Strength = 5;
    public int Dexterity = 5;
    public int Intelligence = 5;
    public int Stamina = 5;
    public int XpRequired;
    public float CharactAttackSpeed = 1;
    [Range(0, 1)] public float CharacterCriticalChance;
    [Range(1, 5)] public float CharacterCriticalModifier;
    public float CharacterBaseMovementSpeed = 5;
    public float CharacterBaseRotationSpeed = 55;
    public float CharacterPushObjectsForce = 5;
    public float CharacterJumpForce = 15;

}
