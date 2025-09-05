using Assets.Scripts.Enemies.Abilities.Interfaces;
using UnityEngine;
using Assets.Scripts.Combat_Logic;
namespace Assets.Scripts.Enemies.Abilities
{
    public abstract class SpecialAbilityBase : MonoBehaviour, ISpecialAbility
    {
        [SerializeField] private float cooldownDuration = 10f;
        private readonly Cooldown cooldown = new();

        public abstract string AbilityName { get; }
        public abstract float Duration { get; }
        public abstract int Priority { get; }
        public abstract bool IsActive { get; protected set; }
        public bool IsReady => cooldown.IsReady;

        public abstract bool ShouldRotateToPlayer();
        public abstract void StartAbility();
        public abstract void StopAbility();

        public void TickCooldown(float deltaTime) => cooldown.Tick(deltaTime);

        protected void ResetCooldown() => cooldown.Start(cooldownDuration);
    }
}
