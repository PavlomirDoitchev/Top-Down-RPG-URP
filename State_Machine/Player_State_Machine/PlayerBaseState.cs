using UnityEngine;
using Assets.Scripts.State_Machine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public abstract class PlayerBaseState : State
    {
         
        protected PlayerStateMachine _playerStateMachine;
        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this._playerStateMachine = stateMachine;
        }
       
        /// <summary>
        /// Move with Input Reading
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="deltaTime"></param>
        protected void Move(Vector3 movement, float deltaTime) 
        {
            _playerStateMachine.CharacterController.Move((movement + _playerStateMachine.ForceReceiver.Movement) * deltaTime);
        }
        /// <summary>
        /// Move without Input Reading
        /// </summary>
        /// <param name="deltaTime"></param>
        protected void Move(float deltaTime) 
        {
            Move(Vector3.zero, deltaTime);
        }
    }
}
