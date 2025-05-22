using UnityEngine;
using Assets.Scripts.Player;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class OrkAttackState : EnemyBaseState
    {
        public OrkAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.CrossFadeInFixedTime("attacking", .1f);
            //SetWeaponActive(true);
            SetEnemyDamage();
        }

        public override void UpdateState(float deltaTime)
        {
            RotateToPlayer(deltaTime);
            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position) > _enemyStateMachine.MeleeAttackDistance
                && _enemyStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .75f)
            {
                _enemyStateMachine.ChangeState(new OrkChaseState(_enemyStateMachine));
            }
            
            
            if (_enemyStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .95f)
            {
                _enemyMelee.EnemyClearHitEnemies();
            }
            if (PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentHealth() <= 0)
            {
                _enemyStateMachine.ChangeState(new OrkIdleState(_enemyStateMachine));
            }
        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
        }
    }
}
