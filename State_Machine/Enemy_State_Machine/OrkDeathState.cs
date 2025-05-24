
using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class OrkDeathState : EnemyBaseState
    {
        public OrkDeathState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
            _enemyStateMachine.Animator.CrossFadeInFixedTime("dying 2", .1f);
            BecomeUntargtable();
        }
        public override void ExitState()
        {
        }

        public override void UpdateState(float deltaTime)
        {
        }
    }
}
