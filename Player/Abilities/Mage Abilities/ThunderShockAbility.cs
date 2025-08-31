namespace Assets.Scripts.Player.Abilities.Mage_Abilities
{
    public class ThunderShockAbility : Skills
    {
        public override void UseSkill()
        {
            base.UseSkill();
            //PlayerManager.Instance.PlayerStateMachine.PlayerStats.UseResource(cost);
            NotifyObservers();
        }
    }
}
