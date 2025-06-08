using Assets.Scripts.Enemies;
using UnityEngine;
using UnityEngine.AI;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyStateMachine : StateMachine
    {

        public EnemyType EnemyType;
        public EnemyStateTypes _enemyStateTypes;
        public GameObject EquippedWeapon;
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        //[field: SerializeField] public Collider BodyCollider { get; private set; } //disabled when dead
        [Header("Make sure to check for null if using patrol!")]
        [field: SerializeField] public PatrolPath PatrolPath { get; private set; }
        [field: SerializeField] public float PatrolDwellTime { get; private set; }
        public int CurrentWaypointIndex { get; set; } = 0;
        public Vector3 OriginalPosition { get; set; }

        #region Enemy AI Stats
        [Header("AI Stats")]
        [field: SerializeField] public float RunningSpeed { get; private set; }
        [field: SerializeField] public float WalkingSpeed { get; private set; }
        [field: SerializeField] public float EnragedSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float AggroRange { get; private set; }
        [field: SerializeField] public float ChaseDistance { get; private set; }
        [field: SerializeField] public float MaxDistanceFromOrigin { get; private set; }
        [field: SerializeField] public float SuspicionTime { get; private set; }
        [field: SerializeField] public float AttackDistance { get; private set; }
        [field: SerializeField] public float AttackDistanceToleranceBeforeChasing { get; private set; }
        [field: SerializeField] public int AttackIndex { get; private set; } = 0; //used to determine which attack animation to play
        [field: SerializeField] public bool CanBecomeEnraged { get; set; } = false; 

        #endregion

        #region Enemy Stats
        [Header("Enemy Stats")]
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
        [field: SerializeField] public string WalkAnimationName { get; private set; } = "walk";
        [field: SerializeField] public string RunAnimationName { get; private set; } = "run";
        [field: SerializeField] public string[] AttackAnimationName { get; private set; }
        [field: SerializeField] public string DeathAnimationName { get; private set; } = "Death";
        [field: SerializeField] public string HitAnimationName { get; private set; } = "hit";
        [field: SerializeField] public string StunnedAnimationName { get; private set; } = "stunned";
        [field: SerializeField] public string EnragedAnimationName { get; private set; } = "Roar";
        #endregion

        #region Global Flags
        public bool ShouldDie { get; set; } = false;
        public bool IsStunned { get; set; } = false;
        public bool IsEnraged { get; set; } = false; 
        #endregion
        private void Start()
        {
            Agent.speed = RunningSpeed;
            OriginalPosition = this.transform.position;
            
            ChangeState(new EnemyIdleState(this));
        }
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AggroRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, ChaseDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, AttackDistance);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, AttackDistanceToleranceBeforeChasing);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(OriginalPosition, MaxDistanceFromOrigin);
        }
        public void TwoHitCombo() 
        {
            if (this.CurrentState is EnemyAttackState) 
            {
                Animator.CrossFadeInFixedTime(AttackAnimationName[AttackIndex], .1f);
                AttackIndex = 1 - AttackIndex; // Toggle between 0 and 1 for combo attacks
            }
        }
    }
}
