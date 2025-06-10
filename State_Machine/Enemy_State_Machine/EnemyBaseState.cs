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
        protected PlayerManager playerManager => PlayerManager.Instance;
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
                _enemyStateMachine.ChangeState(new EnemyPlayerIsDeadState(_enemyStateMachine));
            }
            if (_enemyStateMachine.IsEnraged && _enemyStateMachine.CanBecomeEnraged)
            {
                _enemyStateMachine.ChangeState(new EnemyEnragedState(_enemyStateMachine));
                return true;
            }
            if (_enemyStateMachine.ShouldStartAttacking)
            {
                switch (_enemyStateMachine.EnemyType)
                {
                    case EnemyType.Melee:
                        _enemyStateMachine.ShouldStartAttacking = false;
                        _enemyStateMachine.ChangeState(new EnemyMeleeAttackState(_enemyStateMachine));
                        break;
                    case EnemyType.Ranged:
                        _enemyStateMachine.ShouldStartAttacking = false;
                        _enemyStateMachine.ChangeState(new EnemyRangedAttackState(_enemyStateMachine));

                        break;
                    case EnemyType.MeleeRanged:
                        _enemyStateMachine.ShouldStartAttacking = false;
                        _enemyStateMachine.ChangeState(new EnemyRangedAttackState(_enemyStateMachine));
                        break;
                }
            }
            if (_enemyStateMachine.IsStunned)
            {
                _enemyStateMachine.ChangeState(new EnemyStunnedState(_enemyStateMachine));
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if the enemy can see the player within a certain distance.
        /// Use for Idle, Wander , and Patrol states to determine if the enemy has aggro based on line of sight and parameter distance.
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        protected bool CanSeePlayer(float distance)
        {
            Transform player = playerManager.PlayerStateMachine.transform;
            Vector3 directionToPlayer = (player.position - _enemyStateMachine.transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(_enemyStateMachine.transform.position, player.position);

            if (distanceToPlayer > distance)
                return false;

            if (Vector3.Angle(_enemyStateMachine.transform.forward, directionToPlayer) < _enemyStateMachine.ViewAngle / 2f)
            {

                // Check for obstruction
                if (!Physics.Raycast(_enemyStateMachine.transform.position + Vector3.up * 1.5f, directionToPlayer, distanceToPlayer, _enemyStateMachine.ObstacleMask))
                {
                    Debug.DrawLine(_enemyStateMachine.transform.position + Vector3.up * 1.5f,
                    PlayerManager.Instance.PlayerStateMachine.transform.position,
                    Color.green, 0.1f);
                    // Confirm player
                    if ((1 << player.gameObject.layer & _enemyStateMachine.TargetMask) != 0)
                        return true;
                }
            }
            Debug.DrawLine(_enemyStateMachine.transform.position + Vector3.up * 1.5f, player.position + Vector3.up * 1.5f, Color.red, 0.1f);
            return false;

        }
        /// <summary>
        /// Checks if the player is in line of sight of the enemy. No distance parameter, so it will always check the full distance to the player.
        /// Use for Chase and Ranged Attack states to determine if the enemy can see the player.
        /// </summary>
        /// <returns></returns>
        protected bool HasLineOfSight()
        {
            Transform player = playerManager.PlayerStateMachine.transform;
            Vector3 directionToPlayer = (player.position - _enemyStateMachine.transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(_enemyStateMachine.transform.position, player.position);
            if (Vector3.Angle(_enemyStateMachine.transform.forward, directionToPlayer) < _enemyStateMachine.ViewAngle / 2f)
            {

                // Check for obstruction
                if (!Physics.Raycast(_enemyStateMachine.transform.position + Vector3.up * 1.5f, directionToPlayer, distanceToPlayer, _enemyStateMachine.ObstacleMask))
                {
                    Debug.DrawLine(_enemyStateMachine.transform.position + Vector3.up * 1.5f,
                    PlayerManager.Instance.PlayerStateMachine.transform.position,
                    Color.green, 0.1f);
                    // Confirm player
                    if ((1 << player.gameObject.layer & _enemyStateMachine.TargetMask) != 0)
                        return true;
                }
            }
            Debug.DrawLine(_enemyStateMachine.transform.position + Vector3.up * 1.5f, player.position + Vector3.up * 1.5f, Color.red, 0.1f);
            //Debug.Log("Player is not in line of sight.");
            return false;
        }
        /// <summary>
        /// Rotate to player using the enemy's rotation speed.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected void RotateToPlayer(float deltaTime)
        {
            Vector3 direction = PlayerManager.Instance.PlayerStateMachine.transform.position - _enemyStateMachine.transform.position;
            direction.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _enemyStateMachine.transform.rotation = Quaternion.Slerp(_enemyStateMachine.transform.rotation, targetRotation, deltaTime * _enemyStateMachine.RotationSpeed);
        }
        /// <summary>
        /// Instantly rotates the enemy to face the player.
        /// </summary>
        protected void RotateToPlayer()
        {
            Vector3 direction = PlayerManager.Instance.PlayerStateMachine.transform.position - _enemyStateMachine.transform.position;
            direction.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _enemyStateMachine.transform.rotation = targetRotation;
        }
        /// <summary>
        /// Use later for a teleport ability, like a shadow step or blink.
        /// </summary>
        protected void SnapToPlayer()
        {
            Transform playerTransform = PlayerManager.Instance.PlayerStateMachine.transform;

            float behindDistance = 3f;
            Vector3 offset = -playerTransform.forward * behindDistance;
            Vector3 snapPosition = playerTransform.position + offset;

            snapPosition.y = _enemyStateMachine.transform.position.y;

            _enemyStateMachine.transform.position = snapPosition;
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
        protected void MovementSpeedWalking()
        {
            _enemyStateMachine.Agent.speed = _enemyStateMachine.RunningSpeed;
        }
        protected void MovementSpeedRunning()
        {
            _enemyStateMachine.Agent.speed = _enemyStateMachine.RunningSpeed;
        }
        protected void MovementSpeedEnraged()
        {
            _enemyStateMachine.Agent.speed = _enemyStateMachine.EnragedSpeed;
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
        /// <summary>
        /// Used by Move below...
        /// </summary>
        /// <param name="motion"></param>
        /// <param name="deltaTime"></param>
        protected void Move(Vector3 motion, float deltaTime)
        {
            _enemyStateMachine.CharacterController.Move((motion + _enemyStateMachine.ForceReceiver.Movement) * deltaTime);
        }
        /// <summary>
        /// Moves the enemy using the ForceReceiver's movement vector.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected bool MeleeEnemy()
        {
            if (_enemyStateMachine.EnemyType == EnemyType.Ranged
                || _enemyStateMachine.EnemyType == EnemyType.MeleeRanged)
            {
                return false;
            }
            return true;
        }
    }
}