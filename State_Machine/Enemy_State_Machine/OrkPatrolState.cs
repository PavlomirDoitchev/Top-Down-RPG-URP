using UnityEngine;
using Assets.Scripts.Enemies;
using Assets.Scripts.Player;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class OrkPatrolState : EnemyBaseState
    {
        public int _currentWaypointIndex = 0;
        private float _timeToWaitAtWaypoint = 0f;
        private bool _isDwelling = false;
        public OrkPatrolState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = false;
            _enemyStateMachine.Animator.CrossFadeInFixedTime("walking", .1f);
            _enemyStateMachine.Agent.speed = _enemyStateMachine.WalkingSpeed;
            _enemyStateMachine.Agent.SetDestination(_enemyStateMachine.PatrolPath.GetWaypoint(_currentWaypointIndex));
        }

        public override void UpdateState(float deltaTime)
        {
            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) < _enemyStateMachine.AggroRange)
            {
                _enemyStateMachine.ChangeState(new OrkChaseState(_enemyStateMachine));
            }

            if (AtWaypoint() && !_isDwelling)
            {
                _isDwelling = true;
                _timeToWaitAtWaypoint = 0f;
                _enemyStateMachine.Agent.isStopped = true;
                _enemyStateMachine.Animator.Play("idle");
            }

            if (_isDwelling)
            {
                _timeToWaitAtWaypoint += deltaTime;

                if (_timeToWaitAtWaypoint >= _enemyStateMachine.PatrolDwellTime)
                {
                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _enemyStateMachine.PatrolPath.GetWaypointCount();

                    Vector3 nextWaypoint = _enemyStateMachine.PatrolPath.GetWaypoint(_currentWaypointIndex);
                    _enemyStateMachine.Agent.SetDestination(nextWaypoint);
                    _enemyStateMachine.Agent.isStopped = false;
                    _enemyStateMachine.Animator.CrossFadeInFixedTime("walking", 0.1f);

                    _isDwelling = false;
                }
                //if (AtWaypoint())
                //{
                //    _currentWaypointIndex++;
                //    _timeToWaitAtWaypoint += deltaTime;

                //    if (_currentWaypointIndex == _enemyStateMachine.PatrolPath.GetWaypointCount())
                //        _currentWaypointIndex = 0;

                //    Vector3 nextWaypoint = _enemyStateMachine.PatrolPath.GetWaypoint(_currentWaypointIndex);

                //    if (_timeToWaitAtWaypoint >= _enemyStateMachine.PatrolDwellTime)
                //    {
                //        _enemyStateMachine.Animator.CrossFadeInFixedTime("walking", .1f);
                //        _enemyStateMachine.Agent.isStopped = false;
                //        _enemyStateMachine.Agent.SetDestination(nextWaypoint);
                //        _timeToWaitAtWaypoint = 0f;
                //    }
                //    else if (_timeToWaitAtWaypoint < _enemyStateMachine.PatrolDwellTime)
                //    {
                //        _enemyStateMachine.Agent.isStopped = true;
                //        _enemyStateMachine.Animator.Play("idle");
                //    }

                //}

            }
        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
            ResetMovementSpeed();
        }

       

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(_enemyStateMachine.PatrolPath.GetWaypoint(_currentWaypointIndex), _enemyStateMachine.transform.position);
            return distanceToWaypoint < _enemyStateMachine.Agent.stoppingDistance;
        }
    }

}

