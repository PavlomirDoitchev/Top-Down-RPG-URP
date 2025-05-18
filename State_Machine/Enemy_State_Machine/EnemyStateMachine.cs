using UnityEngine;
using UnityEngine.AI;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyStateMachine : StateMachine
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }

        [Header("Stats")]
        [field: SerializeField] public float ChaseDistance { get; private set; }

        private void Start()
        {
            ChangeState(new OrkIdleState(this));
        }
    }
}
