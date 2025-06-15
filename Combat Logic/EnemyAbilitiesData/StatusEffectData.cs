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
		Bleed,
		Freeze,
		Stun
	}
	public enum ModifyMainStatType
	{
		Stamina,
		Strength,
		Dexterity,
		Intellect
	}
	public StatusEffectType statusEffectType;
	public string Name;

	[Header("DOT Properties")]
	public int DOTDamage;
	public float DOTDuration;
	public float DOTInterval;

	[Header("Extra Effects")]
	[Tooltip("Slow amount in %. At 2, Controls get reversed. Use for Confusion style effects")]
	[Range(0, 2)] public float SlowAmount; // 0 = no slow, 1 = 100% slow, 2 = reversed controls 
	[Range(-1, 1)] public float ModifyAttackSpeed = 0f;
	public ModifyMainStatType modifyStat;
	public int ModifyMainStat = 0;

	[Header("Stacking Options")]
	public bool IsStackable = false;
	public int MaxStacks = 1;
	public float StackRefreshDuration = 0f; // refreshes duration per stack

	[Header("Visuals")]
	public ParticleSystem VFX;
	public DamageNumber DamageNumberPrefab;
}
