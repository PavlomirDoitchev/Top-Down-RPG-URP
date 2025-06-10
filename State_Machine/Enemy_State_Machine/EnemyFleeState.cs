
using Assets.Scripts.Player;
using UnityEngine.AI;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyFleeState : EnemyBaseState
    {
        public EnemyFleeState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = false;
            _enemyStateMachine.Agent.speed = _enemyStateMachine.RunningSpeed;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.RunAnimationName, .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            if (CheckForGlobalTransitions()) return;
            if (Vector3.Distance(_enemyStateMachine.transform.position, PlayerManager.Instance.PlayerStateMachine.transform.position)
                > _enemyStateMachine.FleeingRange)
            {
                _enemyStateMachine.ChangeState(_enemyStateMachine.PreviousState);
                return;
            }
            else
            {
                Vector3 directionAway = (_enemyStateMachine.transform.position - PlayerManager.Instance.PlayerStateMachine.transform.position).normalized;

                Vector3 newDestination = _enemyStateMachine.transform.position + directionAway * _enemyStateMachine.FleeingDistanceAmount;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(newDestination, out hit, 1.0f, NavMesh.AllAreas))
                {
                    _enemyStateMachine.Agent.SetDestination(hit.position);
                }
            }
        }

        public override void ExitState()
        {
            _enemyStateMachine.Agent.isStopped = true;
        }
    }

}

