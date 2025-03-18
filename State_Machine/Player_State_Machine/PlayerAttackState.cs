//using UnityEngine;
//namespace Assets.Scripts.State_Machine.Player_State_Machine
//{
//    public class PlayerAttackState : PlayerBaseState
//    {
//        private float previousFrameTime;
//        private bool alreadyAppliedForce;
//        private AttackData attack;
//        private bool rotationLocked = false;
//        private Quaternion lockedRotation;
//        public PlayerAttackState(PlayerStateMachine stateMachine, int attackId) : base(stateMachine)
//        {
//            attack = _playerStateMachine.AttackData[attackId];
//        }

//        public override void EnterState()
//        {
//            _playerStateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
//        }
//        public override void UpdateState(float deltaTime)
//        {
//            if (!rotationLocked)
//            {
//                RotateToMouse(deltaTime);
//            }
//            else
//            {
//                _playerStateMachine.transform.rotation = lockedRotation;
//            }
//            if (!rotationLocked && _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
//            {
//                rotationLocked = true;
//                lockedRotation = _playerStateMachine.transform.rotation;
//            }
//            Move(deltaTime);
//            float normalizeTime = GetNormalizedTime(_playerStateMachine.Animator);
//            if (normalizeTime >= previousFrameTime && normalizeTime < 1f)
//            {
//                if (normalizeTime >= attack.ForceTime)
//                    TryApplyForce();
//                if (_playerStateMachine.InputManager.IsAttacking)
//                    CheckCombo(normalizeTime);
//            }
//            else
//            {
//                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
//            }

//            previousFrameTime = normalizeTime;
//        }

//        public override void ExitState()
//        {
//        }
//        private void CheckCombo(float normalizedTime)
//        {
//            if (attack.ComboStateIndex == -1) return;
//            if (normalizedTime < attack.ComboAttackTime) return;
//            _playerStateMachine.ChangeState(new PlayerAttackState(_playerStateMachine, attack.ComboStateIndex));

//        }
//        private void TryApplyForce()
//        {
//            if (alreadyAppliedForce) return;
//            _playerStateMachine.ForceReceiver.AddForce(_playerStateMachine.transform.forward * attack.Force);
//            alreadyAppliedForce = true;
//        }
//        private float GetNormalizedTime(Animator animator)
//        {
//            AnimatorStateInfo currentInfo = _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0);
//            AnimatorStateInfo nextInfo = _playerStateMachine.Animator.GetNextAnimatorStateInfo(0);
//            if (animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
//            {
//                return nextInfo.normalizedTime;
//            }
//            else if (!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
//            {
//                return currentInfo.normalizedTime;
//            }
//            else
//            {
//                return 0;
//            }
//        }
//    }
//}