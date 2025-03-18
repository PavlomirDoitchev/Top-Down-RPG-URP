using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Scriptable Objects/CharacterStatsSO")]
public class CharacterStatsSO : ScriptableObject
{
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int Stamina;

    public float CharactAttackSpeed;
    public float CharacterCriticalChance;

    public float CharacterBaseMovementSpeed;
    public float CharacterBaseRotationSpeed;
    public float CharacterPushObjectsForce;
    public float CharacterJumpForce;

}
