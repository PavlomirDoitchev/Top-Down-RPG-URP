using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffectHandler : MonoBehaviour, IEffectable
{
	private class ActiveEffect
	{
		public StatusEffectData Data;
		public float ElapsedTime;
		public float NextTickTime;
		public int StackCount;
		public float CurrentSlowAmount => Data.SlowAmount * StackCount;
		public float CurrentAttackSpeed => Data.ModifyAttackSpeed * StackCount;
		public int CurrentStatChange => Data.ModifyMainStat * StackCount;
	}

	private Dictionary<StatusEffectData.StatusEffectType, ActiveEffect> activeEffects = new();
	[SerializeField] EnemyStateMachine _enemyStateMachine;

    void Update()
    {
		if (activeEffects != null && activeEffects.Count > 0)
			HandleEffect();
	}
	public void ApplyEffect(StatusEffectData data)
	{
		if (activeEffects.TryGetValue(data.statusEffectType, out var existing))
		{
			if (data.IsStackable)
			{
				existing.StackCount = Mathf.Min(existing.StackCount + 1, data.MaxStacks);
				existing.ElapsedTime = 0f;
				existing.NextTickTime = 0f;

				if (data.StackRefreshDuration > 0)
					existing.ElapsedTime = 0;
				//Debug.Log($"Stacked {data.statusEffectType} to {existing.StackCount} stacks on player.");
			}
			else
			{
				existing.ElapsedTime = 0f;
				existing.NextTickTime = 0f;
			}
		}
		else
		{
			activeEffects[data.statusEffectType] = new ActiveEffect
			{
				Data = data,
				StackCount = 1,
				ElapsedTime = 0f,
				NextTickTime = 0f
			};
		}
	}

	public void HandleEffect()
	{

		List<StatusEffectData.StatusEffectType> toRemove = new();

		foreach (var kvp in activeEffects)
		{

			var effect = kvp.Value;
			effect.ElapsedTime += Time.deltaTime;

			if (effect.ElapsedTime >= effect.Data.DOTDuration)
			{
				toRemove.Add(kvp.Key);
				continue;
			}
			if (effect.Data.DOTDamage != 0 && effect.ElapsedTime > effect.NextTickTime)
			{

				effect.NextTickTime += effect.Data.DOTInterval;
				int totalDamage = (effect.Data.DOTDamage
					+ Random.Range(PlayerManager.Instance.PlayerStateMachine.Weapon.minDamage, PlayerManager.Instance.PlayerStateMachine.Weapon.maxDamage))
					* effect.StackCount;
				_enemyStateMachine.TakeDamage(totalDamage, false);
				effect.Data.DamageNumberPrefab.Spawn(transform.position, totalDamage);

			}
		}

		foreach (var key in toRemove)
		{
			activeEffects.Remove(key);

		}
	}

	private float CalculateTotalSlow()
	{
		float maxSlow = 0f;
		foreach (var effect in activeEffects.Values)
		{
			if (effect.Data.SlowAmount > 0)
			{
				maxSlow = Mathf.Max(maxSlow, effect.CurrentSlowAmount);

			}
		}
		return maxSlow;
	}
}
