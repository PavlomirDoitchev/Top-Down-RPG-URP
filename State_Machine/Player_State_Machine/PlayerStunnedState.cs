using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerStunnedState : PlayerBaseState
    {
        float _stunDuration = 2f;
        float elapsedTime;
        public PlayerStunnedState(PlayerStateMachine stateMachine, float stunDuration) : base(stateMachine)
        {
            _stunDuration = stunDuration;
            _playerStateMachine.ForceReceiver.ResetForces();
        }

        public override void EnterState()
        {
            Debug.Log("Stunned State");
            base.EnterState();
            //TransitionToDeathState();
            _playerStateMachine.Animator.CrossFadeInFixedTime("Unarmed-Stunned", 0.1f);
            elapsedTime = 0f;
        }

        public override void UpdateState(float deltaTime)
        {
            //TransitionToDeathState();
            Move(deltaTime);
            elapsedTime += deltaTime;
            if(elapsedTime >= _stunDuration)
            {
                _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
                return;
            }
        }

        public override void ExitState()
        {
        }
        void TransitionToDeathState() 
        {
            if (_playerStateMachine.PlayerStats.GetCurrentHealth() <= 0)
            {
                _playerStateMachine.ChangeState(new PlayerDeathState(_playerStateMachine));
                return;
            }
        }
    }

}

