using UnityEngine;
using Assets.Scripts.Player;
using System.Threading;
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
            _enemyStateMachine.AggrevateNearbyEnemies();
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

            _enemyStateMachine.AbilityClock.Tick(deltaTime);

            if (_enemyStateMachine.AbilityClock.TimeElapsed >= _enemyStateMachine.SpecialAbilityThreshold &&
                _enemyStateMachine.SpecialAbilityCooldown.IsReady)
            {
                _enemyStateMachine.PreviousCombatState = this;
                _enemyStateMachine.ChangeState(new EnemySpecialAbilityState(_enemyStateMachine));
                _enemyStateMachine.AbilityClock.Reset();
                return;
            }
            if(_enemyStateMachine.ShouldRotateInMeleeAttack)
                RotateToPlayer(deltaTime);
            
            if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
                > _enemyStateMachine.AttackRangeToleranceBeforeChasing && _enemyStateMachine.EnemyType == EnemyType.Melee)
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
            }
            else if (Vector3.Distance(PlayerManager.Instance.PlayerStateMachine.transform.position, _enemyStateMachine.transform.position)
                > _enemyStateMachine.AttackRange && _enemyStateMachine.EnemyType == EnemyType.MeleeRanged)
                _enemyStateMachine.ChangeState(new EnemyRangedAttackState(_enemyStateMachine));
        }
        public override void ExitState()
        {
            //_enemyStateMachine.OnAbilityCheck -= HandleAbilityCheck;
            ResetAnimationSpeed();
            MovementSpeedRunning();
        }
     

    }
}