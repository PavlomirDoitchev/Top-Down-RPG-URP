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
    

    [Header("Resource Settings")]
    public int maxResource = 100;
    [Header("XP TO NEXT LEVEL")]
    public int XpRequired;
    private void OnValidate()
    {
        ApplyClassResourceType(); 
    }

    public void ApplyClassResourceType()
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
        ApplyClassResourceType();
    }

    public CharacterClass GetCharacterClass()
    {
        return characterClass;
    }
}
