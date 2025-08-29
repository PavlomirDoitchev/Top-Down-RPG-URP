using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyChaseState : EnemyBaseState
    {
        float timer = 0f;
        float resetTimer = 0.1f; // Reset timer to avoid immediate state changes
        public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = false;
            _enemyStateMachine.CheckForFriendlyInCombat = false;
            
            RotateToPlayer();

            _enemyStateMachine.Agent.speed = _enemyStateMachine.IsEnraged
                ? _enemyStateMachine.EnragedSpeed
                : _enemyStateMachine.RunningSpeed;

            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.RunAnimationName, 0.1f);
            
        }

        public override void UpdateState(float deltaTime)
        {
            timer += deltaTime;
            if (CheckForGlobalTransitions()) return;
            if(timer < resetTimer) return; // Wait for the reset timer before processing further
            Vector3 playerPos = PlayerManager.Instance.PlayerStateMachine.transform.position;
            _enemyStateMachine.Agent.SetDestination(playerPos);

            // --- Transitions ---
            float distToOrigin = Vector3.Distance(_enemyStateMachine.OriginalPosition, _enemyStateMachine.transform.position);
            if (distToOrigin > _enemyStateMachine.MaxDistanceFromOrigin && !_enemyStateMachine.IsEnraged)
            {
                _enemyStateMachine.ChangeState(new ReturnToOriginState(_enemyStateMachine));
                return;
            }

            if (_enemyStateMachine.CanShadowStep &&
                Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
                > _enemyStateMachine.ChaseDistance * _enemyStateMachine.ShadowStepThresholdDistance &&
                _enemyStateMachine.PreviousState is EnemyMeleeAttackState)
            {
                SnapToPlayer();
                return;
            }

            if (!_enemyStateMachine.IsEnraged &&
                (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) > _enemyStateMachine.ChaseDistance
                || !HasLineOfSight()))
            {
                _enemyStateMachine.ChangeState(new EnemySuspicionState(_enemyStateMachine));
                return;
            }

            float distanceToPlayer = Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position);
            if (distanceToPlayer < _enemyStateMachine.AttackRangeToleranceBeforeChasing && HasLineOfSight() && MeleeEnemy())
            {
                _enemyStateMachine.ChangeState(new EnemyMeleeAttackState(_enemyStateMachine));
                return;
            }
            else if (distanceToPlayer < _enemyStateMachine.RangedAttackRange && HasLineOfSight() && !MeleeEnemy())
            {
                _enemyStateMachine.ChangeState(new EnemyRangedAttackState(_enemyStateMachine));
                return;
            }
        }

        public override void ExitState()
        {
            MovementSpeedRunning();
        }


    }
}
