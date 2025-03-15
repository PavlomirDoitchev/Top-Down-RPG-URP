using UnityEngine;
using Assets.Scripts.State_Machine;
using System;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class TestState : PlayerBaseState
    {
        float timer = 5;
        public TestState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void EnterState()
        {
            Debug.Log("EnterState Enter");
        }
        public override void UpdateState(float deltaTime)
        {
            timer -= deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerStateMachine.ChangeState(new TestState(_playerStateMachine));
            }
            Debug.Log(timer.ToString("F1"));
        }

        public override void ExitState()
        {
            Debug.Log("TestState Exit");
        }

    }
}
