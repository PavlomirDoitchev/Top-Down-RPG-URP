using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Enemies.Abilities.Interfaces;
using UnityEngine;

public abstract class SpecialAbilityBase : MonoBehaviour, ISpecialAbility
{
    [SerializeField] private float cooldownDuration = 10f;
    private readonly Cooldown cooldown = new();

    // NEW: controlled by BossPhaseSwitcher
    [HideInInspector] public bool PhaseEnabled = false;

    public abstract string AbilityName { get; }
    public abstract float Duration { get; }
    public abstract int Priority { get; }
    public abstract bool IsActive { get; protected set; }
    public bool IsReady => cooldown.IsReady;

    public abstract bool ShouldRotateToPlayer();
    public abstract void StartAbility();
    public abstract void StopAbility();

    // tick cooldown runs only if this ability is active (abilities can early-return)
    public virtual void TickCooldown(float deltaTime) => cooldown.Tick(deltaTime);

    public void ResetCooldown() => cooldown.Start(cooldownDuration);
}