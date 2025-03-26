using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class FighterQ : Skills
    {
        [SerializeField] ParticleSystem particle;
        public override void UseSkill()
        {
            base.UseSkill();
            var playerStats = PlayerManager.Instance.playerStateMachine._PlayerStats;
            var playerState = PlayerManager.Instance.playerStateMachine;
            var skillManager = SkillManager.Instance;
            Instantiate(particle, transform.position, Quaternion.identity, playerState.transform);
            playerStats.UseResource(GetSkillCost());
            Debug.Log($"Used {playerState.qAbilityData[playerState.QAbilityRank].name} Rank: {playerState.QAbilityRank} Cost: {cost}. Remaining {playerStats.GetResourceType()}: {playerStats.GetCurrentResource()}");

            playerState.ChangeState(new FighterAbilityQState(playerState));
        }

    }
}
