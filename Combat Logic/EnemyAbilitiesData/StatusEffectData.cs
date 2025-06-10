using DamageNumbersPro;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StatusEffectData", menuName = "Enemy Spell Data/StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    public enum StatusEffectType
    {
        Poison,
        Burn,
        Slow,
        Freeze,
        Stun
    }

    public StatusEffectType statusEffectType;
    public string Name;

    [Header("DOT Properties")]
    public int DOTDamage;
    public float DOTDuration;
    public float DOTInterval;

    [Header("Slow/Other Effects")]
    public float SlowAmount;

    [Header("Stacking Options")]
    public bool IsStackable = false;
    public int MaxStacks = 1;
    public float StackRefreshDuration = 0f; // refreshes duration per stack

    [Header("Visuals")]
    public GameObject VFX;
    public DamageNumber DamageNumberPrefab;
}
