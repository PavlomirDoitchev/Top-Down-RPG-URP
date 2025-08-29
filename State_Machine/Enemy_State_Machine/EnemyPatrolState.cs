using UnityEngine;
using Assets.Scripts.Enemies;
using Assets.Scripts.Player;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyPatrolState : EnemyBaseState
    {
        private float _timeToWaitAtWaypoint = 0f;
        private bool _isDwelling = false;

        public EnemyPatrolState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = false;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.WalkAnimationName, .1f);
            _enemyStateMachine.Agent.speed = _enemyStateMachine.WalkingSpeed;
            _enemyStateMachine.Agent.SetDestination(_enemyStateMachine.PatrolPath.GetWaypoint(_enemyStateMachine.CurrentWaypointIndex));
        }

        public override void UpdateState(float deltaTime)
        {
            if(CheckForGlobalTransitions()) return;

            if (CanSeePlayer(_enemyStateMachine.AggroRange) || _enemyStateMachine.CheckForFriendlyInCombat)
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
            
            _enemyStateMachine.OriginalPosition = _enemyStateMachine.transform.position;
            if (AtWaypoint() && !_isDwelling)
            {
                DwellingOnPatrolWaypoint();
            }

            if (_isDwelling)
            {
                _timeToWaitAtWaypoint += deltaTime;

                if (_timeToWaitAtWaypoint >= _enemyStateMachine.PatrolDwellTime)
                {
                    GoToNextPatrolWaypoint();
                }
            }
        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
            MovementSpeedRunning();
        }

        private void DwellingOnPatrolWaypoint()
        {
            _isDwelling = true;
            _timeToWaitAtWaypoint = 0f;
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.Play(_enemyStateMachine.IdleAnimationName);
        }

        private void GoToNextPatrolWaypoint()
        {
            _enemyStateMachine.CurrentWaypointIndex = (_enemyStateMachine.CurrentWaypointIndex + 1) % _enemyStateMachine.PatrolPath.GetWaypointCount();

            Vector3 nextWaypoint = _enemyStateMachine.PatrolPath.GetWaypoint(_enemyStateMachine.CurrentWaypointIndex);
            _enemyStateMachine.Agent.SetDestination(nextWaypoint);
            _enemyStateMachine.Agent.isStopped = false;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.WalkAnimationName, 0.1f);

            _isDwelling = false;
        }


        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(_enemyStateMachine.PatrolPath.GetWaypoint(_enemyStateMachine.CurrentWaypointIndex), _enemyStateMachine.transform.position);
            return distanceToWaypoint < _enemyStateMachine.Agent.stoppingDistance;
        }
    }

}

