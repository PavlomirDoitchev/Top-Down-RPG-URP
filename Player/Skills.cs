using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Skills : MonoBehaviour
    {
        [SerializeField] protected int cost;
        [SerializeField] protected float coolDown;
        protected float cooldownTimer;

        public virtual void Update() 
        {
            cooldownTimer -= Time.deltaTime;
        }
        public virtual bool CanUseSkill() 
        {
            if (cooldownTimer <= 0 && cost <= PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentResource())
            {
                UseSkill();
                cooldownTimer = coolDown;
                return true;
            }
            return false;
        }
        public virtual void UseSkill() 
        {
            var playerStats = PlayerManager.Instance.PlayerStateMachine.PlayerStats;
            playerStats.UseResource(GetSkillCost());
        }
        public int GetSkillCost() => this.cost;
    }
}
