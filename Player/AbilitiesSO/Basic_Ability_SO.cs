using UnityEngine;

[CreateAssetMenu(fileName = "Basic_Attack_SO", menuName = "Abilities/Create New Basic Attack Rank")]
public class Basic_Ability_SO : ScriptableObject
{
    public string abilityName;
    public float damageMultiplier;
    public float force;
    //public ParticleSystem VFX;
}
