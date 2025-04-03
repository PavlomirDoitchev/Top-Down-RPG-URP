using System;
public enum BuffType
{
    AttackBoost,
    SpeedBoost,
    DefenseBoost
}
[Serializable]
public class Buff
{
    public BuffType Type;
    public float Duration;
    public float EffectStrength;

    public Buff(BuffType type, float duration, float effectStrength)
    {
        Type = type;
        Duration = duration;
        EffectStrength = effectStrength;
    }
}