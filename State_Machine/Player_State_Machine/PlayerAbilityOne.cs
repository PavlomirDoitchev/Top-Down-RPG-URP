using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerAbilityOne : PlayerBaseState
    {
        private float duration = 1f;
        private float timer = 0f;
        bool alreadyAppliedForce;
        AttackData attack;
        public PlayerAbilityOne(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            attack = _playerStateMachine.AttackData[0];
        }

        public override void EnterState()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("2Hand-Sword-Attack3", .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            Move(deltaTime);
            TryApplyForce();
            timer += deltaTime; 
            if(timer >= duration)
            {
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
            }
        }
        public override void ExitState()
        {
        }
        private void TryApplyForce()
        {
            if (alreadyAppliedForce) return;
            _playerStateMachine.ForceReceiver.AddForce(_playerStateMachine.transform.forward * attack.Force);
            alreadyAppliedForce = true;
        }
    }
}