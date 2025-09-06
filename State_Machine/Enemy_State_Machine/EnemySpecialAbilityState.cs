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
            RotateToPlayer();

            abilities = _enemyStateMachine.GetComponentsInChildren<ISpecialAbility>().ToArray();
        }

        public override void UpdateState(float deltaTime)
        {
            if (_enemyStateMachine.ShouldDie)
            {
                _enemyStateMachine.ChangeState(new EnemyDeathState(_enemyStateMachine));
                return;
            }

            // Tick all abilities
            foreach (var ability in abilities)
            {
                ability.TickCooldown(deltaTime);
            }

            if (chosenAbility == null)
            {
                // Global cooldown must be ready
                if (_enemyStateMachine.SpecialAbilityCooldown.IsReady)
                {
                    var readyAbilities = abilities.Where(a => a.IsReady).ToArray();
                    if (readyAbilities.Length > 0)
                    {
                        chosenAbility = readyAbilities.OrderByDescending(a => a.Priority).First();
                        chosenAbility.StartAbility();
                        _enemyStateMachine.SpecialAbilityCooldown.Start(_enemyStateMachine.SpecialAbilityCooldownDuration);
                    }
                    else
                    {
                        ExitOrReturnToPreviousState();
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            if (chosenAbility != null && !chosenAbility.IsActive)
            {
                ExitOrReturnToPreviousState();
                return;
            }

            if (chosenAbility != null && chosenAbility.ShouldRotateToPlayer())
            {
                RotateToPlayer(deltaTime);
            }
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
