using DamageNumbersPro;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectData", menuName = "Enemy Spell Data/StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    public string Name;
    public int DOTDamage;
    public float DOTDuration;
    public float DOTInterval;
    public float SlowAmount;

    public GameObject VFX;
    public DamageNumber DamageNumberPrefab;
}
