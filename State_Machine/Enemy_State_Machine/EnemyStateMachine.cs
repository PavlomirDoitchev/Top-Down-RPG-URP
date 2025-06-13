using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Enemies;
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyStateMachine : StateMachine, IDamagable
    {
        public PlayerManager player => PlayerManager.Instance;
        public EnemyType EnemyType;
        public EnemyStateTypes EnemyStateTypes;
        public GameObject EquippedWeapon;
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }

        #region Patrol Logic
        [field: SerializeField] public PatrolPath PatrolPath { get; private set; }
        [field: SerializeField] public float PatrolDwellTime { get; private set; }
        public int CurrentWaypointIndex { get; set; } = 0;
        public Vector3 OriginalPosition { get; set; }
        #endregion

        #region Enemy AI Stats
        [Header("AI Stats")]
        [Space(10)]
        public string enemyAIName = "Enemy AI";
		public Cooldown RangedAttackCooldown = new(); //create a new instance of this if you want to track a different ability CD
		[field: SerializeField] public float ViewAngle { get; private set; } = 120f; //degrees
        [field: SerializeField] public LayerMask ObstacleMask { get; private set; }
        [field: SerializeField] public LayerMask TargetMask { get; private set; }
        [field: SerializeField] public float RunningSpeed { get; private set; }
        [field: SerializeField] public float WalkingSpeed { get; private set; }
        [field: SerializeField] public float EnragedSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float AggroRange { get; private set; }
        [field: SerializeField] public float ChaseDistance { get; private set; }
        [field: SerializeField] public float MaxDistanceFromOrigin { get; private set; }
        [field: SerializeField] public float SuspicionTime { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float AttackRangeToleranceBeforeChasing { get; private set; }
        [field :SerializeField] public float RangedAttackRange { get; private set; }
        [field: SerializeField] public float FleeingRange { get; private set; } //should be less than RangedAttackDistance
        [field: SerializeField] public float FleeingDistanceAmount { get; private set; } = 5f; 
        [field: SerializeField] public float RangedAttackCooldownDuration { get; private set; } = 2f; 
        [field: SerializeField] public int AttackIndex { get; private set; } = 0; //used to determine which attack animation to play
        [field: SerializeField]
        [field: Range(0, 1)] public float EnrageThreshold { get; private set; } = 0.5f; //percentage of health at which the enemy becomes enraged
        [field: SerializeField] public bool CanBecomeEnraged { get; set; } = false;
        [field: SerializeField] public bool CanShadowStep { get; set; } = false; 
        [field: SerializeField]
        [field: Range(0.1f, 0.9f)] public float ShadowStepThresholdDistance { get; private set; } = 0.5f;
        [field: SerializeField] public float ShadowStepDistanceAmount { get; private set; } = 3f;
		#endregion

		#region Enemy Stats
		[Header("----------Enemy Stats----------")]
        [Space(10)]
        public string enemyName = "Enemy"; 
        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        [field: SerializeField] public int CurrentHealth { get; private set; }
        [field: SerializeField] public int XpReward { get; private set; } = 1;
        [field: SerializeField]
        [field: Range(0, 1)] public float CriticalChance { get; private set; }
        [field: SerializeField]
        [field: Range(1, 5)] public float CriticalModifier { get; private set; }
        [field: SerializeField]
        [field: Range(0.1f, 5f)] public float BaseAttackSpeed { get; private set; }
        [field: SerializeField]
        [field: Range(1f, 3f)] public float EnragedAttackSpeed { get; private set; }
        [field: SerializeField]
        [field: Range(1, 5)] public float EnragedDamageMultiplier { get; private set; }
        [field: SerializeField] public float KnockBackForce { get; private set; } = 15f;
        [field: SerializeField] public bool ShouldKnockBackPlayer { get; set; }

        #endregion

        #region Animation Variables
        [Header("Animation Names")]
        [field: SerializeField] public string IdleAnimationName { get; private set; } = "idle";
        [field: SerializeField] public string SuspicionAnimationName { get; private set; } = "idle01";
        [field: SerializeField] public string WalkAnimationName { get; private set; } = "walk";
        [field: SerializeField] public string RunAnimationName { get; private set; } = "run";
        [field: SerializeField] public string[] AttackAnimationName { get; private set; }
        [field: SerializeField] public string DeathAnimationName { get; private set; } = "Death";
        [field: SerializeField] public string HitAnimationName { get; private set; } = "hit";
        [field: SerializeField] public string StunnedAnimationName { get; private set; } = "stunned";
        [field: SerializeField] public string EnragedAnimationName { get; private set; } = "Roar";
        [field: SerializeField] public string CastAnimationName { get; private set; } = "cast";
        [field: SerializeField] public string ChannelAnimationName { get; private set; } = "SpellA_Channel";
		[field: SerializeField] public string SnapAnimationName { get; private set; } = "snap";
        #endregion

        #region Global Flags
        public bool ShouldDie { get; set; } = false;
        public bool ShouldReturnToOrigin { get; set; } = false;
        public bool IsStunned { get; set; } = false;
        public bool IsEnraged { get; set; } = false;
        public bool ShouldStartAttacking { get; set; } = false;  
        public bool ShouldShadowStep { get; set; } = false; 
        #endregion
        private void Start()
        {
            Agent.speed = RunningSpeed;
            OriginalPosition = this.transform.position;
            CurrentHealth = MaxHealth;
            ChangeState(new EnemyIdleState(this));
        }
        //private void Update()
        //{
        //    GlobalTimer -= Time.deltaTime;
        //}
        public void TakeDamage(int damage, bool applyImpulse)
        {
            applyImpulse = false;
            CurrentHealth -= damage;
            ShouldStartAttacking = true;
            if (CurrentHealth <= Mathf.RoundToInt(MaxHealth * EnrageThreshold) && CanBecomeEnraged)
                IsEnraged = true;

            //if (CurrentHealth <= MaxHealth * 0.8f && CanShadowStep)
            //    ShouldShadowStep = true;

            if (CurrentHealth <= 0)
            {
                ShouldDie = true;
                player.PlayerStateMachine.PlayerStats.GainXP(XpReward);
            }
        }
        public void TwoHitCombo()
        {
            if (this.CurrentState is EnemyMeleeAttackState)
            {
                Animator.CrossFadeInFixedTime(AttackAnimationName[AttackIndex], .1f);
                AttackIndex = 1 - AttackIndex;
            }
        }
        public void OnDrawGizmosSelected()
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.position, AggroRange);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, FleeingRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, ChaseDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
            //Gizmos.color = Color.magenta;
            //Gizmos.DrawWireSphere(transform.position, AttackRangeToleranceBeforeChasing);
            //Gizmos.color = Color.cyan;
            //Gizmos.DrawWireSphere(transform.position, RangedAttackRange);
            ////Gizmos.color = Color.blue;
            ////Gizmos.DrawWireSphere(OriginalPosition, MaxDistanceFromOrigin);


            //Vector3 position = transform.position;
            //Vector3 forward = transform.forward;

            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(position, AggroRange);

            //Vector3 leftBoundary = Quaternion.Euler(0, -ViewAngle / 2f, 0) * forward * AggroRange;
            //Vector3 rightBoundary = Quaternion.Euler(0, ViewAngle / 2f, 0) * forward * AggroRange;

            //Gizmos.color = Color.blue;
            //Gizmos.DrawLine(position, position + leftBoundary);
            //Gizmos.DrawLine(position, position + rightBoundary);
        }
    }
}
