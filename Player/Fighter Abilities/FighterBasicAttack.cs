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
        /// <summary>
        /// Added in the animation event to enable the trail renderer. Added to the weapon prefab as a component!
        /// </summary>
        void TrailRendererEnabled() 
        {
            PlayerManager.Instance.PlayerStateMachine.EquippedWeapon.GetComponent<TrailRenderer>().emitting = true;
        }
        /// <summary>
        /// Added in the animation event to disable the trail renderer. Added to the weapon prefab as a component!
        /// </summary>
        void TrailRendererDisabled()
        {
            PlayerManager.Instance.PlayerStateMachine.EquippedWeapon.GetComponent<TrailRenderer>().emitting = false;
        }
    }
}
