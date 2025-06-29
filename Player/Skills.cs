using Assets.Scripts.Player.Abilities;
using Assets.Scripts.Utility.UI;
using UnityEngine;

namespace Assets.Scripts.Player
{
  
    public class Skills : Subject
    {	
		public string animationName;
		public string castingAnimationName;
		[field: SerializeField] public ParticleSystem CastingVFX { get; private set; }
		[field: SerializeField] public ParticleSystem SpellVFX { get; private set; }
		[field: SerializeField] public ParticleSystem ImpactVFX { get; private set; }
		[SerializeField] protected int cost;
		[SerializeField] protected float coolDown;
		
        [field: SerializeField] public float CostCheckInterval { get; private set; } = 1f;
		public bool AllowMovementWhileCasting;
		public bool AllowRotationWhileCasting;	
		public bool IsChanneled;
		[field: SerializeField] public float CastTime { get; private set; }

        
        protected float cooldownTimer;

		public virtual void Update() => cooldownTimer -= Time.deltaTime;
		public int GetSkillCost() => this.cost;
		public bool CanChannel() => IsChanneled && PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentResource() >= cost;
		public void ResetCooldown()
		{
			cooldownTimer = coolDown;
			NotifyObservers();
        }
        public float GetCooldownTimer() => cooldownTimer;
        public virtual void UseSkill()
		{
			PlayerManager.Instance.PlayerStateMachine.PlayerStats.UseResource(cost);
            cooldownTimer = coolDown;
			NotifyObservers();
		}

		public virtual bool CanUseSkill()
		{
			if (cooldownTimer <= 0 && cost <= PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentResource())
			{
				return true;
			}
			return false;
		}
		public bool CanUseChanneledSkill()
		{
			if (IsChanneled && cooldownTimer <= 0 && cost <= PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentResource())
			{
				return true;
			}
			return false;
		}
	}
}