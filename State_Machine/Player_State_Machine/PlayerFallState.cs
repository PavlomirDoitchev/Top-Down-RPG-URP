using UnityEngine;
using DamageNumbersPro;
using Assets.Scripts.Player;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerFallState : PlayerBaseState
    {
        private Vector3 momentum;
        private float maxFallSpeed;
        private int fallDamage;
        bool canCastWhileFalling = false;
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            //Debug.Log("Entering fall state");
            _playerStateMachine.Animator.CrossFadeInFixedTime("2Hand-Sword-Fall", 0.1f);
            momentum = _playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
            maxFallSpeed = 0f;
        }

        public override void UpdateState(float deltaTime)
        {
            PlayerMove(deltaTime);
            if (_playerStateMachine.InputManager.AbilitySixInput()
                && _playerStateMachine.Ability_Three_Rank > 0
                && SkillManager.Instance.ThunderShockAbility.CanUseSkill())
            {
                canCastWhileFalling = true;
                _playerStateMachine.Animator.CrossFadeInFixedTime("ThunderShockAirLoop", 0.1f);
                Vector3 slamVelocity = _playerStateMachine.CharacterController.velocity;
                slamVelocity.y = -40f;
                _playerStateMachine.ForceReceiver.AddForce(slamVelocity);

            }
            if (_playerStateMachine.CharacterController.velocity.y < maxFallSpeed)
                maxFallSpeed = _playerStateMachine.CharacterController.velocity.y;


            if (_playerStateMachine.CharacterController.isGrounded)
            {
                if (canCastWhileFalling)
                {
                    CastThunderShock();
                    return;

                }
                if (maxFallSpeed < -20f)
                {
                    fallDamage = Mathf.RoundToInt(Mathf.Abs((maxFallSpeed * _playerStateMachine.PlayerStats.GetMaxHealth()) * 0.01f));
                    _playerStateMachine.PlayerStats.TakeDamage(fallDamage, true);
                }
                if (fallDamage > 0)
                {
                    _playerStateMachine.DamageText[0].Spawn(_playerStateMachine.transform.position, fallDamage);
                }
                if (_playerStateMachine.PlayerStats.GetCurrentHealth() > 0)
                {
                    //_playerStateMachine.Animator.Play("2Hand-Sword-Land");
                    _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
                }
            }
        }
        private void CastThunderShock()
        {
            _playerStateMachine.ChangeState(new CastingAbilityState(_playerStateMachine, SkillManager.Instance.ThunderShockAbility));
        }
        public override void ExitState()
        {
            //Debug.Log("FallState Exit");
        }

    }
}
