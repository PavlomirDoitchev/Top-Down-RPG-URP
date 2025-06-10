
using Assets.Scripts.Player;
using UnityEngine.AI;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyFleeState : EnemyBaseState
    {
        float fleeRange = 0f;
        //float fleeTimer = 5f;

        public EnemyFleeState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = false;
            _enemyStateMachine.Agent.speed = _enemyStateMachine.RunningSpeed;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.RunAnimationName, .1f);
            fleeRange = _enemyStateMachine.FleeingRange;
        }

        public override void UpdateState(float deltaTime)
        {
            if (CheckForGlobalTransitions()) return;
            //fleeTimer -= deltaTime;
            //if(fleeTimer <= 0f)
            //{
            //    _enemyStateMachine.ChangeState(_enemyStateMachine.PreviousState);
            //    return;
            //}
            var playerPos = PlayerManager.Instance.PlayerStateMachine.transform.position;
            var enemyPos = _enemyStateMachine.transform.position;

            if (Vector3.Distance(enemyPos, playerPos) > _enemyStateMachine.FleeingRange * 2/* || fleeTimer <= 0f*/)
            {
                _enemyStateMachine.ChangeState(new EnemyRangedAttackState(_enemyStateMachine));
                return;
            }
            else
            {
                Vector3 directionAway = (enemyPos - playerPos).normalized;

                // Try up to N attempts to find a valid flee path
                const int maxAttempts = 5;
                Vector3 fleeDestination = enemyPos;
                bool validPathFound = false;

                for (int i = 0; i < maxAttempts; i++)
                {
                    // Random angle offset between -45 and 45 degrees
                    float randomAngle = Random.Range(-120f, 120f);
                    Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);
                    Vector3 randomFleeDirection = rotation * directionAway;

                    Vector3 tentativeDestination = enemyPos + randomFleeDirection * _enemyStateMachine.FleeingDistanceAmount;

                    // Sample position on NavMesh close to tentativeDestination
                    if (NavMesh.SamplePosition(tentativeDestination, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                    {
                        tentativeDestination = hit.position;

                        // Check if path is walkable
                        NavMeshPath path = new NavMeshPath();
                        bool pathFound = _enemyStateMachine.Agent.CalculatePath(tentativeDestination, path);

                        if (pathFound && path.status == NavMeshPathStatus.PathComplete)
                        {
                            fleeDestination = tentativeDestination;
                            validPathFound = true;
                            break;
                        }
                    }
                }

                if (!validPathFound)
                {
                    // No valid flee path found, set fleeing range to 0 so enemy can attack
                    fleeRange = 0;
                    return;
                }

                _enemyStateMachine.Agent.SetDestination(fleeDestination);
            }
        }


        public override void ExitState()
        {
            fleeRange = _enemyStateMachine.FleeingRange;
        }
    }

}

