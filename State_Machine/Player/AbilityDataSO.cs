using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/Create New Ability")]
public class AbilityDataSO : ScriptableObject
{
    public string abilityName;
    public float damageMultiplier; 
    public float force;
}