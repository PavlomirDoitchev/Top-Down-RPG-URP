﻿namespace Assets.Scripts.Combat_Logic
{
    public interface IEffectable
    {
        public void ApplyEffect(StatusEffectData _data);
        public void HandleEffect();
    }
}
