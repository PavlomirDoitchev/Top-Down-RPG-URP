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
    }
}
