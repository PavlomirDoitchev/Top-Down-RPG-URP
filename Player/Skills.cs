using UnityEngine;

namespace Assets.Scripts.Player
{
	public class Skills : MonoBehaviour
	{
		public string animationName;
		public string castingAnimationName;
		[field: SerializeField] public ParticleSystem CastingVFX { get; private set; }
		[field: SerializeField] public ParticleSystem SpellVFX { get; private set; }
		[field: SerializeField] public ParticleSystem ImpactVFX { get; private set; }
		[SerializeField] protected int cost;
		[SerializeField] protected float coolDown;
		[field: SerializeField] public float CostCheckInterval { get; private set; } = 0.3f;
		public bool AllowMovementWhileCasting;
		public bool AllowRotationWhileCasting;	
		public bool IsChanneled;
		[field: SerializeField] public float CastTime { get; private set; }
		protected float cooldownTimer;

		public virtual void Update() => cooldownTimer -= Time.deltaTime;
		public virtual void UseSkill() => PlayerManager.Instance.PlayerStateMachine.PlayerStats.UseResource(cost);
		public int GetSkillCost() => this.cost;
		public bool CanChannel() => IsChanneled && PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentResource() >= cost;
		public void ResetCooldown() => cooldownTimer = coolDown;
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
		
	}
}