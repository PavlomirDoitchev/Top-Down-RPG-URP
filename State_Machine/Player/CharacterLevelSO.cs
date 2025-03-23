using UnityEngine;

[CreateAssetMenu(fileName = "New Character Level", menuName = "New Character Level/Create New Character Level")]
public class CharacterLevelSO : ScriptableObject
{
    public enum CharacterClass
    {
        Fighter,
        Rogue,
        Mage
    }
    public enum ResourceType 
    {
        Rage,
        Energy,
        Mana
    }
    [Header("Character Info")]
    public CharacterClass characterClass;
    public ResourceType resourceType;

    [Header("Stats")]
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int Stamina;
    public int XpRequired;
    public float CharactAttackSpeed;
    [Range(0, 1)] public float CharacterCriticalChance;
    [Range(1, 5)] public float CharacterCriticalModifier;
    public float CharacterBaseMovementSpeed;
    public float CharacterBaseRotationSpeed;
    public float CharacterPushObjectsForce;
    public float CharacterJumpForce;

    [Header("Resource Settings")]
    public int maxResource = 100;
    private void OnValidate()
    {
        ApplyClassModifiers(); 
    }

    public void ApplyClassModifiers()
    {
        switch (characterClass)
        {
            case CharacterClass.Fighter:
                resourceType = ResourceType.Rage;
                break;

            case CharacterClass.Rogue:
                resourceType = ResourceType.Energy;
                break;

            case CharacterClass.Mage:
                resourceType = ResourceType.Mana;
                break;

        }
    }
    public ResourceType GetResourceType()
    {
        return resourceType;
    }
    public void SetCharacterClass(CharacterClass newClass)
    {
        characterClass = newClass;
        ApplyClassModifiers();
    }

    public CharacterClass GetCharacterClass()
    {
        return characterClass;
    }
}
