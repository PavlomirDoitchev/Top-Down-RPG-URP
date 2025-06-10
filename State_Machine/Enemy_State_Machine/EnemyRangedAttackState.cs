using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyRangedAttackState : EnemyBaseState
    {
        float cooldDownTimer;
        public EnemyRangedAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
            RotateToPlayer();
            cooldDownTimer = _enemyStateMachine.RangedAttackCooldown;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.CastAnimationName, .1f);
        }

        public override void UpdateState(float deltaTime)
        {

            cooldDownTimer -= deltaTime;
            if (CheckForGlobalTransitions()) return;
            RotateToPlayer(deltaTime);
            if (!HasLineOfSight() || cooldDownTimer < 0f)
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
                return;
            }
            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
                > _enemyStateMachine.RangedAttackRange)
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
                return;
            }

            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
                < _enemyStateMachine.FleeingRange && _enemyStateMachine.EnemyType == EnemyType.Ranged)
            {
                _enemyStateMachine.ChangeState(new EnemyFleeState(_enemyStateMachine));
               
            }
            else if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
                < _enemyStateMachine.AttackRange && _enemyStateMachine.EnemyType == EnemyType.MeleeRanged)
            {
                _enemyStateMachine.ChangeState(new EnemyMeleeAttackState(_enemyStateMachine));
            }
        }

        public override void ExitState()
        {
            ResetAnimationSpeed();
        }
    }
}