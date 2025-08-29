namespace Assets.Scripts.Player.Abilities.Mage_Abilities
{
    public class CenterPullAbility : Skills
    {
        public override void UseSkill()
        {
            base.UseSkill();
            NotifyObservers();
        }
    }
}
