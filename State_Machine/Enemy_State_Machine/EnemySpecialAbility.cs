using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemySpecialAbility : EnemyBaseState
    {
        public EnemySpecialAbility(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
        }
        public override void ExitState()
        {
            throw new NotImplementedException();
        }

        public override void UpdateState(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
