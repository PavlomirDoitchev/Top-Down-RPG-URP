using UnityEngine;

namespace Assets.Scripts.Player.Abilities.Fighter_Abilities
{
    public class LightningShieldAbility : Skills, ICharges
    {
        [field: SerializeField] public int MaxCharges { get; private set; } = 3; 
        [field: SerializeField] public int CurrentCharges { get; set; }
        [SerializeField] float maxDuration = 10f;
        [field: SerializeField] public float Timer { get; set; }

        public override void UseSkill()
        {
            base.UseSkill();
            NotifyObservers(); 
        }

        public int GetChargeCount()
        {
            return CurrentCharges;
        }
        public float GetRemainingTime()
        {
            return Timer;
        }
        public float GetMaxDuration()
        {
            return maxDuration;
        }
    }
}
