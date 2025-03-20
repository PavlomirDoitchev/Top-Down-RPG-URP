using UnityEngine;
namespace Assets.Scripts.State_Machine
{
    public abstract class State
    {
        public virtual void EnterState() { }
        public abstract void UpdateState(float deltaTime);
        public abstract void ExitState();
    }
}