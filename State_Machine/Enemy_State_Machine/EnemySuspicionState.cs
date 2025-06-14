using UnityEngine;
using Assets.Scripts.Player;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    //TODO: Add some wandering to search for player.
    public class EnemySuspicionState : EnemyBaseState
    {
        float _suspicionTimer = 0f;
        bool _isReturningToOrigin = false;
        Vector3 lastSeenPlayerPos;
        public EnemySuspicionState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            //_enemyStateMachine.Agent.isStopped = true;
            //_enemyStateMachine.Animator.CrossFadeInFixedTime("idle", .1f);
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.RunAnimationName, .1f);
            lastSeenPlayerPos = _enemyStateMachine.player.PlayerStateMachine.transform.position;
            _enemyStateMachine.Agent.SetDestination(lastSeenPlayerPos);
        }

        public override void UpdateState(float deltaTime)
        {
            if (CheckForGlobalTransitions()) return;
            
            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
                < _enemyStateMachine.ChaseDistance
                && HasLineOfSight())
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
                return;
            }
            if (!_isReturningToOrigin && Vector3.Distance(lastSeenPlayerPos, _enemyStateMachine.transform.position) <= _enemyStateMachine.Agent.stoppingDistance)
            {
                InvestigateLastPlayerPosition(deltaTime);
                if (_suspicionTimer >= _enemyStateMachine.SuspicionTime)
                {
                    RunBack();

                    if (_enemyStateMachine.PatrolPath == null)
                        _enemyStateMachine.Agent.SetDestination(_enemyStateMachine.OriginalPosition);
                    else if (_enemyStateMachine.EnemyStateTypes == EnemyStateTypes.Patrol)
                        _enemyStateMachine.ChangeState(new EnemyPatrolState(_enemyStateMachine));
                }
            }
            if (_isReturningToOrigin &&
                _enemyStateMachine.PatrolPath == null &&
                Vector3.Distance(_enemyStateMachine.OriginalPosition, _enemyStateMachine.transform.position) <= _enemyStateMachine.Agent.stoppingDistance)
            {
                _enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine));
            }
        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
        }

        private void RunBack()
        {
            _isReturningToOrigin = true;
            _enemyStateMachine.Agent.isStopped = false;

            _enemyStateMachine.Animator.Play(_enemyStateMachine.RunAnimationName);
            _enemyStateMachine.Agent.speed = _enemyStateMachine.RunningSpeed;
            _suspicionTimer = 0f;
        }

        private void InvestigateLastPlayerPosition(float deltaTime)
        {
            _suspicionTimer += deltaTime;
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.Play(_enemyStateMachine.SuspicionAnimationName);
        }
    }
}