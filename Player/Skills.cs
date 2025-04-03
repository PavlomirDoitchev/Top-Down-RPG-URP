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
            if (cooldownTimer <= 0 && cost <= PlayerManager.Instance.PlayerStateMachine._PlayerStats.GetCurrentResource())
            {
                UseSkill();
                cooldownTimer = coolDown;
                return true;
            }
            return false;
        }
        public virtual void UseSkill() { }
        public int GetSkillCost() => this.cost;
    }
}
