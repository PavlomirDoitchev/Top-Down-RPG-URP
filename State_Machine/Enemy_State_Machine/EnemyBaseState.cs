namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine _stateMachine;
        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
             this._stateMachine = stateMachine;
        }
        public override void EnterState() 
        {
            base.EnterState();
        }

       
    }
}
