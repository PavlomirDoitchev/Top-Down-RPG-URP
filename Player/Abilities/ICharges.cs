namespace Assets.Scripts.Player.Abilities
{
    public interface ICharges
    {
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        int GetChargeCount();
        float GetRemainingTime();
        float GetMaxDuration();
    }
}
