namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine _enemyStateMachine;
        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
             this._enemyStateMachine = stateMachine;
        }
        public override void EnterState() 
        {
            base.EnterState();
        }

       
    }
}
