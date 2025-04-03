using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public event Action<Buff> OnBuffApplied;
    public event Action<Buff> OnBuffExpired;
    public event Action<Debuff> OnDebuffApplied;
    public event Action<Debuff> OnDebuffExpired;

    private List<Buff> activeBuffs = new List<Buff>();
    private List<Debuff> activeDebuffs = new List<Debuff>();

    public void ApplyBuff(Buff buff)
    {
        activeBuffs.Add(buff);
        OnBuffApplied?.Invoke(buff);
        StartCoroutine(HandleBuff(buff));
    }

    public void ApplyDebuff(Debuff debuff)
    {
        activeDebuffs.Add(debuff);
        OnDebuffApplied?.Invoke(debuff);
        StartCoroutine(HandleDebuff(debuff));
    }

    private IEnumerator HandleBuff(Buff buff)
    {
        ApplyBuffEffect(buff, true);
        yield return new WaitForSeconds(buff.Duration);
        ApplyBuffEffect(buff, false);
        activeBuffs.Remove(buff);
        OnBuffExpired?.Invoke(buff);
    }

    private IEnumerator HandleDebuff(Debuff debuff)
    {
        ApplyDebuffEffect(debuff, true);
        yield return new WaitForSeconds(debuff.Duration);
        ApplyDebuffEffect(debuff, false);
        activeDebuffs.Remove(debuff);
        OnDebuffExpired?.Invoke(debuff);
    }

    private void ApplyBuffEffect(Buff buff, bool isApplying)
    {
        float modifier = isApplying ? buff.EffectStrength : -buff.EffectStrength;

        switch (buff.Type)
        {
            case BuffType.AttackBoost:
                ModifyAttack(modifier);
                break;
            case BuffType.SpeedBoost:
                ModifySpeed(modifier);
                break;
            case BuffType.DefenseBoost:
                ModifyDefense(modifier);
                break;
        }
    }

    private void ApplyDebuffEffect(Debuff debuff, bool isApplying)
    {
        float modifier = isApplying ? debuff.EffectStrength : -debuff.EffectStrength;

        switch (debuff.Type)
        {
            case DebuffType.Slow:
                ModifySpeed(modifier);
                break;
            case DebuffType.Weaken:
                ModifyAttack(modifier);
                break;
            case DebuffType.Stun:
                ApplyStun(isApplying);
                break;
        }
    }

    private void ModifyAttack(float amount) => Debug.Log($"Enemy Attack modified by {amount}");
    private void ModifySpeed(float amount) => Debug.Log($"Enemy Speed modified by {amount}");
    private void ModifyDefense(float amount) => Debug.Log($"Enemy Defense modified by {amount}");
    private void ApplyStun(bool isApplying) => Debug.Log(isApplying ? "Enemy Stunned!" : "Enemy Recovered!");
}
