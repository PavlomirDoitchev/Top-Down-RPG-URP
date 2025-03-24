using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Skills : MonoBehaviour
    {
        [SerializeField] protected float coolDown;
        protected float cooldownTimer;
        [SerializeField] int cost;
        public virtual void Update() 
        {
            cooldownTimer -= Time.deltaTime;
        }
        public virtual bool CanUseSkill() 
        {
            if (cooldownTimer < 0 && cost <= PlayerManager.Instance.playerStateMachine._PlayerStats.GetCurrentResource()) 
            {
                UseSkill();
                cooldownTimer = coolDown;
                return true;
            }
            return false;
        }
        public virtual void UseSkill() 
        {

        }
        public int GetSkillCost() 
        {
            return this.cost;
        }
    }
}
