using Assets.Scripts.Enemies.Abilities;
using Assets.Scripts.Enemies.Abilities.Interfaces;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemySpecialAbilityState : EnemyBaseState
    {
        private ISpecialAbility chosenAbility;

        public EnemySpecialAbilityState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            _enemyStateMachine.Agent.isStopped = true;
            var abilities = _enemyStateMachine.GetComponentsInChildren<ISpecialAbility>().ToArray();
            var readyAbilities = abilities.Where(a => a.IsReady).ToArray();

            if (readyAbilities.Length > 0)
            {
                chosenAbility = readyAbilities.OrderByDescending(a => a.Priority).First();
                chosenAbility.StartAbility();
                
            }
            else
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
            }
            RotateToPlayer();
            
        }

        public override void UpdateState(float deltaTime)
        {
            //CheckForGlobalTransitions();
            if(_enemyStateMachine.ShouldDie)
            {
                _enemyStateMachine.ChangeState(new EnemyDeathState(_enemyStateMachine));
                return;
            }
            if (chosenAbility == null) return;
            if (!chosenAbility.IsActive)
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));

            if (chosenAbility.ShouldRotateToPlayer())
            {
                RotateToPlayer(deltaTime);
            }
        }

        public override void ExitState()
        {
            chosenAbility?.StopAbility();
            _enemyStateMachine.Agent.isStopped = false;
        }
    }
}
