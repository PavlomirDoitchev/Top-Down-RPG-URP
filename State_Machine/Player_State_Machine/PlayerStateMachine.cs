using UnityEngine;
using Assets.Scripts.State_Machine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerStateMachine : StateMachine
    {
        
        private void Start()
        {
            ChangeState(new TestState(this));
        }
      
    }
}