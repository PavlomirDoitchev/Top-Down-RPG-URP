
namespace Assets.Scripts.Player.Abilities.Fighter_Abilities
{
	public class RainOfFireAbility : Skills
	{
		public override void UseSkill()
		{
			base.UseSkill();
			NotifyObservers();
		}
	}
}
