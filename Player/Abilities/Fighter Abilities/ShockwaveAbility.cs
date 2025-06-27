
namespace Assets.Scripts.Player.Abilities.Fighter_Abilities
{
	public class ShockwaveAbility : Skills
	{
		public override void UseSkill()
		{
			base.UseSkill();
			//PlayerManager.Instance.PlayerStateMachine.PlayerStats.UseResource(cost);
            NotifyObservers();
        }
	}
}