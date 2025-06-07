using UnityEngine;
using Assets.Scripts.Player;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyAttackState : EnemyBaseState
    {
        public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
            //_enemyStateMachine.Agent.speed = _enemyStateMachine.WalkingSpeed;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.AttackAnimationName, .1f);
            SetAttackSpeed(1f);
        }

        public override void UpdateState(float deltaTime)
        {
            //_enemyStateMachine.Agent.SetDestination(PlayerManager.Instance.PlayerStateMachine.transform.position);
            RotateToPlayer(deltaTime);

            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) > _enemyStateMachine.AttackDistance
                && _enemyStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .75f)
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
            }

            if (PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentHealth() <= 0)
            {
                _enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine));
            }
        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
            ResetMovementSpeed();
        }
    }
}