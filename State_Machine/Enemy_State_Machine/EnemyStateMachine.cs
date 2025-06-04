using UnityEngine;
using UnityEngine.AI;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyStateMachine : StateMachine
    {
        public GameObject EquippedWeapon;
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }

        [Header("Stats")]
        [field: SerializeField] public float RotationSpeed { get; private set; }    
        [field: SerializeField] public float AggroRange { get; private set; }
        [field: SerializeField] public float ChaseDistance { get; private set; }
        [field: SerializeField] public float SuspicionTime { get; private set; }
        [field: SerializeField] public float AttackDistance { get; private set; }
        [field: SerializeField]
        [field: Range(0, 1)] public float CriticalChance { get; private set; }
        [field: SerializeField] 
        [field: Range(1,5)] public float CriticalModifier { get; private set; }
        [field: SerializeField] public Collider BodyCollider { get; private set; }
        public Vector3 OriginalPosition { get; private set; }
        private void Start()
        {
            OriginalPosition = this.transform.position;
            ChangeState(new OrkIdleState(this));
        }
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AggroRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, ChaseDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, AttackDistance);
        }
    }
}
