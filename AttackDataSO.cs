using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackData", menuName = "Stats/Attack Data")]
public class AttackDataSO : ScriptableObject
{
    public string attackName;
    public float damageMultiplier; 
    public float force;
}