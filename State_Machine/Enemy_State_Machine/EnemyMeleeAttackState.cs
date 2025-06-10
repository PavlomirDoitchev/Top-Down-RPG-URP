using UnityEngine;
using Assets.Scripts.Player;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyMeleeAttackState : EnemyBaseState
    {
        public EnemyMeleeAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = false;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.AttackAnimationName[0], .1f);
            RotateToPlayer();
            

            if (_enemyStateMachine.IsEnraged)
            {
                SetAttackSpeed(_enemyStateMachine.EnragedAttackSpeed);
                MovementSpeedEnraged();
            }
            else 
            { 
                SetAttackSpeed(1f);
                MovementSpeedWalking();
            }
        }

        public override void UpdateState(float deltaTime)
        {
            if (CheckForGlobalTransitions()) return;
            
            RotateToPlayer(deltaTime);

            _enemyStateMachine.Agent.SetDestination(PlayerManager.Instance.PlayerStateMachine.transform.position);

            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
                > _enemyStateMachine.AttackRangeToleranceBeforeChasing && _enemyStateMachine.EnemyType == EnemyType.Melee)
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
            }
            else if(Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
                > _enemyStateMachine.AttackRange && _enemyStateMachine.EnemyType == EnemyType.MeleeRanged)
                _enemyStateMachine.ChangeState(new EnemyRangedAttackState(_enemyStateMachine));
        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
            MovementSpeedRunning();
        }
    }
}