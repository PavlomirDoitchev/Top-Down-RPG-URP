using Assets.Scripts.Enemies.Abilities.Interfaces;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemySpecialAbilityState : EnemyBaseState
    {
        private ISpecialAbility chosenAbility;
        private ISpecialAbility[] abilities;

        public EnemySpecialAbilityState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;

            var abilities = _enemyStateMachine.GetComponentsInChildren<ISpecialAbility>()
                                             .Where(a => a.IsReady)
                                             .OrderByDescending(a => a.Priority)
                                             .ToArray();

            if (abilities.Length == 0 || !_enemyStateMachine.SpecialAbilityCooldown.IsReady)
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
                return;
            }

            chosenAbility = abilities.First();
            chosenAbility.StartAbility();
            _enemyStateMachine.SpecialAbilityCooldown.Start(_enemyStateMachine.SpecialAbilityCooldownDuration);

            RotateToPlayer();
        }

        public override void UpdateState(float deltaTime)
        {
            if (_enemyStateMachine.ShouldDie)
            {
                _enemyStateMachine.ChangeState(new EnemyDeathState(_enemyStateMachine));
                return;
            }

            if (chosenAbility == null) return;

            if (!chosenAbility.IsActive)
            {
                ExitOrReturnToPreviousState();
            }

            if (chosenAbility.ShouldRotateToPlayer())
                RotateToPlayer(deltaTime);
        }

        public override void ExitState()
        {
            chosenAbility?.StopAbility();
            _enemyStateMachine.Agent.isStopped = false;
        }
        private void ExitOrReturnToPreviousState()
        {
            if (_enemyStateMachine.PreviousCombatState != null)
            {
                _enemyStateMachine.ChangeState(_enemyStateMachine.PreviousCombatState);
                _enemyStateMachine.PreviousCombatState = null;
            }
            else
            {
                _enemyStateMachine.ChangeState(new EnemyChaseState(_enemyStateMachine));
            }
        }
    }
}
