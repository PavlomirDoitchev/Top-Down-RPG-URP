using Assets.Scripts.Player;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class OrkChaseState : EnemyBaseState
    {
        public OrkChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            _enemyStateMachine.Agent.isStopped = false;
            _enemyStateMachine.Agent.speed = 3.5f;

        }

        public override void UpdateState(float deltaTime)
        {
            _enemyStateMachine.Agent.SetDestination(PlayerManager.Instance.PlayerStateMachine.transform.position);

        }

        public override void ExitState()
        {

        }
    }
}
