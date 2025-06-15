
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
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.DeathAnimationName, .1f);
            BecomeUntargtable();
            _enemyStateMachine.CharacterController.enabled = false;
            
            //_enemyStateMachine.BodyCollider.enabled = false;
            _enemyStateMachine.Agent.enabled = false;
        }
        public override void UpdateState(float deltaTime)
        {
            
            _enemyStateMachine.transform.position = Vector3.Lerp(_enemyStateMachine.transform.position, _enemyStateMachine.transform.position + Vector3.down * 0.1f, deltaTime);
			timer += deltaTime;
            if(timer > 10f)
			{
				// Destroy the enemy after 10 seconds
				Object.Destroy(_enemyStateMachine.gameObject);
			}
		}
        public override void ExitState()
        {
        }

    }
}
