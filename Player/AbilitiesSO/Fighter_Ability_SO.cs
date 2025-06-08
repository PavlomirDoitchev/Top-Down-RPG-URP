using UnityEngine;

[CreateAssetMenu(fileName = "Basic_Attack_SO", menuName = "Abilities/Create New Basic Attack Rank")]
public class Fighter_Ability_SO : ScriptableObject
{
    public string abilityName;
    public float damageMultiplier;
    public float force;
    public float knockbackForce;
    public Buff[] buffs;
    public AbilityDebuff[] debuffs;  
    //public ParticleSystem VFX;
}
