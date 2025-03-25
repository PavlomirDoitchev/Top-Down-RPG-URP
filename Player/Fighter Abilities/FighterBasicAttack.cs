using Assets.Scripts.State_Machine.Player_State_Machine;

namespace Assets.Scripts.Player
{
    public class FighterBasicAttack : Skills
    {
        public override void UseSkill()
        {
            base.UseSkill();
            var playerStats = PlayerManager.Instance.playerStateMachine._PlayerStats;
            var playerState = PlayerManager.Instance.playerStateMachine;
            playerStats.UseResource(GetSkillCost());
            playerState.ChangeState(new FighterBasicAttackChainOne(playerState));

        }
    }
}
