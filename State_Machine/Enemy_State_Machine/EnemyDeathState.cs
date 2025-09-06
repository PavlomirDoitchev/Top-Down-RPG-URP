
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyDeathState : EnemyBaseState
    {
        float timer = 0;
        public EnemyDeathState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EnterState()
        {
            base.EnterState();

            // Spawn health pickup with chance
            if (_enemyStateMachine.HealthPickup != null && Random.value < _enemyStateMachine.HealthPickupChance)
            {
                Vector3 spawnPosition = _enemyStateMachine.transform.position + Vector3.up * 0.5f; // spawn slightly above ground
                GameObject pickupInstance = GameObject.Instantiate(_enemyStateMachine.HealthPickup, spawnPosition, Quaternion.identity);

               // pickupInstance.SetActive(true);
            }

            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.DeathAnimationName, .1f);
            BecomeUntargtable();
            _enemyStateMachine.CharacterController.enabled = false;
            PlayerManager.Instance.PlayerStateMachine.PlayerStats.GainXP(_enemyStateMachine.XpReward);
            _enemyStateMachine.Agent.enabled = false;
        }
        public override void UpdateState(float deltaTime)
        {

            _enemyStateMachine.transform.position = Vector3.Lerp(_enemyStateMachine.transform.position, _enemyStateMachine.transform.position + Vector3.down * 0.1f, deltaTime);
            timer += deltaTime;
            if (timer > 10f)
            {
                _enemyStateMachine.RaiseDeathEvent();
                Object.Destroy(_enemyStateMachine.gameObject);
            }
        }
        public override void ExitState()
        {
        }

    }
}
