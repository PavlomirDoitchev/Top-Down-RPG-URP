using System;

[Serializable]
public class AbilityDebuff
{
    public DebuffType debuffType;
    public float duration;
    public float effectStrength;

    public AbilityDebuff(DebuffType type, float duration, float effectStrength)
    {
        this.debuffType = type;
        this.duration = duration;
        this.effectStrength = effectStrength;
    }
}