using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Scriptable Objects/CharacterStatsSO")]
public class CharacterStatsSO : ScriptableObject
{
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int Stamina;
    public float BaseMovementSpeed;
    public float BaseRotationSpeed;
    public float PushObjectsForce;
    public float JumpForce;
}
