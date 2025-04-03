using System;
public enum DebuffType
{
    Slow,
    Weaken,
    Stun
}
[Serializable]
public class Debuff
{
    public DebuffType Type;
    public float Duration;
    public float EffectStrength;

    public Debuff(DebuffType type, float duration, float effectStrength)
    {
        Type = type;
        Duration = duration;
        EffectStrength = effectStrength;
    }
}