using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWanderState : EnemyBaseState
{
    //private float _wanderRadius = 25f;
    private float _dwellTime = 2f;
    private float _waitTimer = 0f;
    private bool _isWaiting = false;

    public EnemyWanderState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        _enemyStateMachine.Agent.isStopped = false;
        _enemyStateMachine.Agent.speed = _enemyStateMachine.WalkingSpeed;
        _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.WalkAnimationName, 0.1f);
        SetNewWanderDestination();
    }

    public override void UpdateState(float deltaTime)
    {
        if(CheckForGlobalTransitions()) return;

        if (CanSeePlayer(_enemyStateMachine.AggroRange))
        {
            _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
            return;
        }
        if (!_isWaiting && !_enemyStateMachine.Agent.pathPending && _enemyStateMachine.Agent.remainingDistance < _enemyStateMachine.Agent.stoppingDistance)
        {
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.Play(_enemyStateMachine.IdleAnimationName);
            _isWaiting = true;
            _waitTimer = 0f;
        }

        if (_isWaiting)
        {
            _waitTimer += deltaTime;
            if (_waitTimer >= _dwellTime)
            {
                SetNewWanderDestination();
                _enemyStateMachine.Agent.isStopped = false;
                _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.WalkAnimationName, 0.1f);
                _isWaiting = false;
            }
        }
    }
    private void SetNewWanderDestination()
    {
        Vector3 origin = _enemyStateMachine.OriginalPosition;
        float wanderRadius = 20f;
        int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection.y = 0f; 
            Vector3 candidate = origin + randomDirection;

            if (Vector3.Distance(candidate, origin) <= _enemyStateMachine.MaxDistanceFromOrigin)
            {
                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
                {
                    _enemyStateMachine.Agent.SetDestination(hit.position);
                    return;
                }
            }
        }
    }

    //private void SetNewWanderDestination()
    //{
    //    
    //    Vector3 randomDirection = Random.insideUnitSphere * _wanderRadius;
    //    randomDirection += _enemyStateMachine.transform.position;

    //    if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _wanderRadius, NavMesh.AllAreas))
    //    {
    //        _enemyStateMachine.Agent.SetDestination(hit.position);
    //    }
    //}

    public override void ExitState()
    {
        _enemyStateMachine.Agent.isStopped = false;
    }
}
