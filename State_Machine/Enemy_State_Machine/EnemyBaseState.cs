using Assets.Scripts.State_Machine.Player_State_Machine;
using static Assets.Scripts.State_Machine.Player_State_Machine.PlayerBaseState;
using UnityEngine;
using Assets.Scripts.Player;
using Assets.Scripts.Enemies;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine _enemyStateMachine;
      
        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
            this._enemyStateMachine = stateMachine;
        }
        public override void EnterState()
        {
            base.EnterState();
            Debug.Log($"Entering state: {this.GetType().Name}");
        }
        protected bool CheckForGlobalTransitions() 
        {
            if (_enemyStateMachine.ShouldDie)
            {
                _enemyStateMachine.ChangeState(new EnemyDeathState(_enemyStateMachine));
                return true;
            }
            if (PlayerManager.Instance.PlayerStateMachine.PlayerStats.GetCurrentHealth() <= 0) 
            {
                _enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine));
            }
            if (_enemyStateMachine.IsEnraged && _enemyStateMachine.CanBecomeEnraged)
            {
                _enemyStateMachine.ChangeState(new EnemyEnragedState(_enemyStateMachine));
                return true;
            }
            if (_enemyStateMachine.IsStunned)
            {
                _enemyStateMachine.ChangeState(new EnemyStunnedState(_enemyStateMachine));
                return true;
            }
            return false;
        }
        protected void RotateToPlayer(float deltaTime)
        {
            Vector3 direction = PlayerManager.Instance.PlayerStateMachine.transform.position - _enemyStateMachine.transform.position;
            direction.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _enemyStateMachine.transform.rotation = Quaternion.Slerp(_enemyStateMachine.transform.rotation, targetRotation, deltaTime * _enemyStateMachine.RotationSpeed);
        }
        /// <summary>
        /// Change the layer of the enemy to "Default" so it can't be targeted by the player.
        /// </summary>
        protected void BecomeUntargtable()
        {
            _enemyStateMachine.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        /// <summary>
        /// Change the layer of the enemy to "Enemy" so it can be targeted by the player.
        /// </summary>
        protected void BecomeTargetable()
        {
            _enemyStateMachine.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
        protected void ResetMovementSpeed()
        {
            _enemyStateMachine.Agent.speed = _enemyStateMachine.RunningSpeed;
        }
        /// <summary>
        /// Keep speed at 1f for normal speed.
        /// </summary>
        /// <param name="speed"></param>
        protected void SetAttackSpeed(float speed) 
        {
            _enemyStateMachine.Animator.speed = _enemyStateMachine.BaseAttackSpeed * speed;
        }
        protected void ResetAnimationSpeed()
        {
            _enemyStateMachine.Animator.speed = 1f;
        }
       
    }
}