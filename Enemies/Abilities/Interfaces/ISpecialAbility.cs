namespace Assets.Scripts.Enemies.Abilities.Interfaces
{
    public interface ISpecialAbility
    {
        string AbilityName { get; }
        float Duration { get; }
        bool IsActive { get; }
        int Priority { get; }
        bool IsReady { get; }        
        void TickCooldown(float deltaTime); 

        bool ShouldRotateToPlayer();
        void StartAbility();
        void StopAbility();
    }
}
