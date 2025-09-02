using Assets.Scripts.State_Machine;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Mount_State_Machine
{
    public abstract class MountBaseState : State
    {
        protected readonly MountStateMachine stateMachine;

        protected MountBaseState(MountStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log($"[MOUNT] Entering state: {GetType().Name}");
        }
    }
}
