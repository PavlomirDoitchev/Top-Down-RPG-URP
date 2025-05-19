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
        [field: SerializeField] public float AggroRange { get; private set; }
        [field: SerializeField] public float ChaseDistance { get; private set; }
        [field: SerializeField] public float MeleeAttackDistance { get; private set; }
        [field: SerializeField]
        [field: Range(0, 1)] public float CriticalChance { get; private set; }
        [field: SerializeField] 
        [field: Range(1,5)] public float CriticalModifier { get; private set; }

        private void Start()
        {
            ChangeState(new OrkIdleState(this));
        }
    }
}
