using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class FighterDodgeState : PlayerBaseState
    {
        Vector3 dashDirection;
        Vector3 dashVelocity;
        float rotationSpeed;
        //float dashSpeed = 10f;
        public FighterDodgeState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void EnterState()
        {
            base.EnterState();
            rotationSpeed = _playerStateMachine.PlayerStats.RotationSpeed + 50f;
            _playerStateMachine.Animator.speed = 1.2f;
            _playerStateMachine.Animator.Play("2Hand-Sword-DiveRoll-Forward1");
            _playerStateMachine.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisionWithEnemy");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {

                Vector3 targetPoint = hit.point;
                targetPoint.y = _playerStateMachine.transform.position.y;
                dashDirection = (targetPoint - _playerStateMachine.transform.position).normalized;
               _playerStateMachine.transform.rotation = Quaternion.LookRotation(targetPoint - _playerStateMachine.transform.position);
                
            }
            
            else
            {
                dashDirection = _playerStateMachine.transform.forward;
            }

            dashVelocity = dashDirection * _playerStateMachine.PlayerStats.BaseMovementSpeed;
            dashVelocity.y = Physics.gravity.y;
        }



        public override void UpdateState(float deltaTime)
        {
            //if (_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.05f)
            //    RotateToMouse(deltaTime);

            Move(dashVelocity, deltaTime);
            if (_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
            }

        }
        public override void ExitState()
        {
            _playerStateMachine.gameObject.layer = LayerMask.NameToLayer("Player");
            ResetAnimationSpeed();
        }
    }
}
