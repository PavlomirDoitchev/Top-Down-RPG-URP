using UnityEngine;
using Assets.Scripts.Enemies;
using Assets.Scripts.Player;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class OrkPatrolState : EnemyBaseState
    {
        public int _currentWaypointIndex = 0;
        public OrkPatrolState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = false;
            _enemyStateMachine.Animator.CrossFadeInFixedTime("walking", .1f);
            _enemyStateMachine.Agent.speed *= 0.5f;
            _enemyStateMachine.Agent.SetDestination(_enemyStateMachine.PatrolPath.GetWaypoint(_currentWaypointIndex));
        }

        public override void UpdateState(float deltaTime)
        {
            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) < _enemyStateMachine.AggroRange)
            {
                _enemyStateMachine.ChangeState(new OrkChaseState(_enemyStateMachine));
            }


            if (AtWaypoint())
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex == _enemyStateMachine.PatrolPath.GetWaypointCount())
                {
                    _currentWaypointIndex = 0;
                }
                Vector3 nextWaypoint = _enemyStateMachine.PatrolPath.GetWaypoint(_currentWaypointIndex);
                _enemyStateMachine.Agent.SetDestination(nextWaypoint);
            }

        }

        public override void ExitState()
        {
            ResetAnimationSpeed();
            ResetSpeed();
        }

        private void ResetSpeed()
        {
            _enemyStateMachine.Agent.speed = _enemyStateMachine.Agent.speed / 0.5f;
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(_enemyStateMachine.PatrolPath.GetWaypoint(_currentWaypointIndex), _enemyStateMachine.transform.position);
            return distanceToWaypoint < _enemyStateMachine.Agent.stoppingDistance;
        }
    }

}

