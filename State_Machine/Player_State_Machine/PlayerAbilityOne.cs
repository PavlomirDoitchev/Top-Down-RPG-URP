using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerAbilityOne : PlayerBaseState
    {
        private float duration = 1f;
        private float timer = 0f;
        bool alreadyAppliedForce;
        AttackData attack;
        private bool rotationLocked = false;
        private Quaternion lockedRotation;
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
            if (!rotationLocked)
            {
                RotateToMouse(deltaTime);
            }
            else
            {
                _playerStateMachine.transform.rotation = lockedRotation;
            }
            if (!rotationLocked && _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
            {
                rotationLocked = true;
                lockedRotation = _playerStateMachine.transform.rotation;
            }

            Move(deltaTime);
            TryApplyForce();
            //timer += deltaTime;
            //if (timer >= duration)
            //{
            //    _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
            //}
            if (_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
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