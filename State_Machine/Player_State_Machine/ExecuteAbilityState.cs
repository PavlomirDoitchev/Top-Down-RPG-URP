using UnityEngine;
using Assets.Scripts.Player;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class ExecuteAbilityState : PlayerBaseState
    {
        private readonly Skills _skill;
        float timer;
        float checkInterval;
        public ExecuteAbilityState(PlayerStateMachine stateMachine, Skills skill) : base(stateMachine)
        {
            _skill = skill;
        }

        public override void EnterState()
        {
            base.EnterState();
            ResetAnimationSpeed();
            RotateToMouse();

            if (_skill.IsChanneled)
            {
                timer = _skill.CastTime;
                _playerStateMachine.Animator.Play(_skill.castingAnimationName);
                checkInterval = _skill.CostCheckInterval;
            }
            else 
            {
                _playerStateMachine.Animator.speed = 2f;
                _playerStateMachine.Animator.Play(_skill.animationName);
            }
        }

        public override void UpdateState(float deltaTime)
        {
            Move(deltaTime);
            if (_skill.AllowRotationWhileCasting)
                RotateToMouse();
            
            if (_skill.IsChanneled)
            {
                Channel(deltaTime);
            }
        }
        public override void ExitState()
        {
            ResetAnimationSpeed();
        }
        private void Channel(float deltaTime)
        {
            timer -= deltaTime;

            PlayerMove(deltaTime);
            if (!_skill.AllowMovementWhileCasting
                && _playerStateMachine.CharacterController.velocity != Vector3.zero)
            {
                //Debug.Log("Cannot move while channeling!");
                _skill.CastingVFX.gameObject.SetActive(false);
                _skill.ResetCooldown();
                _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
                return;
            }

            _skill.CastingVFX.gameObject.SetActive(true);
            
            checkInterval -= deltaTime;
            if (checkInterval <= 0f)
            {
                _skill.UseSkill();
                //Debug.Log($"Using skill: {_skill.animationName}");
                checkInterval = _skill.CostCheckInterval;
                if (_playerStateMachine.PlayerStats.GetCurrentResource() < _skill.GetSkillCost())
                {
                    _skill.CastingVFX.gameObject.SetActive(false);
                    _skill.ResetCooldown();
                    _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
                    return;
                }
            }

            //Debug.Log($"Channeled...: {timer}");
            if (timer <= 0)
            {
                _skill.ResetCooldown();
                _skill.CastingVFX.gameObject.SetActive(false);
                _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
                return;
            }
        }
    }
}
