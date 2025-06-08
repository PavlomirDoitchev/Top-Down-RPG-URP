using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;
namespace Assets.Scripts.Player
{
    public class FighterBasicAttack : Skills
    {
        [SerializeField]
        public override void UseSkill()
        {
            base.UseSkill();
            var playerStats = PlayerManager.Instance.PlayerStateMachine.PlayerStats;
            var playerState = PlayerManager.Instance.PlayerStateMachine;
            playerStats.UseResource(GetSkillCost());
            playerState.ChangeState(new FighterBasicAttackChainOne(playerState));
        }
    }
}
