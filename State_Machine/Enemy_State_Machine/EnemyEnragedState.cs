using UnityEngine;
using Assets.Scripts.Player;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyEnragedState : EnemyBaseState
    {
        public EnemyEnragedState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.CanBecomeEnraged = false; 
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.EnragedAnimationName, .1f);
            BecomeUntargtable();
        }
        public override void UpdateState(float deltaTime)
        {
            //if (CheckForGlobalTransitions()) return;
            // play enraged animation
            if (_enemyStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .9f) 
            {
                BecomeTargetable();
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
            }

        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
            MovementSpeedRunning();
        }
    }
}
