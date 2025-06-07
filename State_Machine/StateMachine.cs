using UnityEngine;

namespace Assets.Scripts.State_Machine
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State currentState;
        protected State CurrentState => currentState; 

        private void Update()
        {
            currentState?.UpdateState(Time.deltaTime);
        }

        public void ChangeState(State newState)
        {
            currentState?.ExitState();
            currentState = newState;
            currentState?.EnterState();
        }
    }
}