using UnityEngine;
using Assets.Scripts.Player;
using System.Threading;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyMeleeAttackState : EnemyBaseState
    {
        float timer = 0f;
        float setNewPosTimer = .3f;
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
            RotateToPlayer(deltaTime);
            timer += deltaTime;
            if (timer > setNewPosTimer)
            {
                CircleAroundPlayer();
                timer = 0f;
            }
            //_enemyStateMachine.Agent.SetDestination(PlayerManager.Instance.PlayerStateMachine.transform.position);
            //Vector3 repulsion = Vector3.zero;
            //Collider[] nearbyEnemies = Physics.OverlapSphere(_enemyStateMachine.transform.position, 1.0f, LayerMask.GetMask("Enemy"));
            //foreach (var col in nearbyEnemies)
            //{
            //    if (col.gameObject == this._enemyStateMachine.gameObject) continue;
            //    repulsion += (_enemyStateMachine.transform.position - col.transform.position).normalized;
            //}
            //_enemyStateMachine.Agent.Move(repulsion.normalized * Time.deltaTime * 0.5f);
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
            ResetAnimationSpeed();
            MovementSpeedRunning();
        }
        private void CircleAroundPlayer()
        {
            float circleRadius = 2f; // Adjust based on spacing
            Vector3 targetPosition = _enemyStateMachine.GetCirclePositionAroundPlayer(circleRadius);
            _enemyStateMachine.Agent.SetDestination(targetPosition);
        }
    }
}