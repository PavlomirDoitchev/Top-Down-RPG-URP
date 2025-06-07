
using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyDeathState : EnemyBaseState
    {
        public EnemyDeathState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.DeathAnimationName, .1f);
            BecomeUntargtable();
            _enemyStateMachine.BodyCollider.enabled = false;
            _enemyStateMachine.Agent.enabled = false;
        }
        public override void ExitState()
        {
        }

        public override void UpdateState(float deltaTime)
        {
        }
    }
}
