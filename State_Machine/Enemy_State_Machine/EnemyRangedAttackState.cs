using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyRangedAttackState : EnemyBaseState
    {
        float cooldDown = 1f;
        public EnemyRangedAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
            RotateToPlayer();
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.CastAnimationName, .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            cooldDown -= deltaTime;
            if (CheckForGlobalTransitions()) return;
            RotateToPlayer(deltaTime);
            if (!HasLineOfSight() || cooldDown < 0f) 
            {
                _enemyStateMachine.ChangeState(_enemyStateMachine.PreviousState); 
                return;
            }
            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
                > _enemyStateMachine.RangedAttackDistance)
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
                return;
            }
            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
                < _enemyStateMachine.AttackDistance)
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