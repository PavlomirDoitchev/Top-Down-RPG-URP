using Assets.Scripts.Enemies.Abilities.Interfaces;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemySpecialAbilityState : EnemyBaseState
    {
        private ISpecialAbility currentAbility;
        private ISpecialAbility[] allAbilities;
        private float stateTimer = 0f;
        private float maxStateDuration = 5f;

        public EnemySpecialAbilityState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.Agent.isStopped = true;
            stateTimer = 0f;

            // Always use the abilities from the state machine
            allAbilities = _enemyStateMachine.SpecialAbilities
                .OrderByDescending(a => a.Priority)
                .ToArray();

            if (allAbilities == null || allAbilities.Length == 0)
            {
                ReturnToPreviousState();
                return;
            }

            StartNextAbility();
            RotateToPlayer();
        }

        public override void UpdateState(float deltaTime)
        {
            stateTimer += deltaTime;

            if (_enemyStateMachine.ShouldDie)
            {
                _enemyStateMachine.ChangeState(new EnemyDeathState(_enemyStateMachine));
                return;
            }

            foreach (var ability in allAbilities)
                ability.TickCooldown(deltaTime);

            if (currentAbility != null)
            {
                if (currentAbility.ShouldRotateToPlayer())
                    RotateToPlayer(deltaTime);

                if (!currentAbility.IsActive)
                    StartNextAbility();
            }
            else
            {
                StartNextAbility();
                if (currentAbility == null)
                    ReturnToPreviousState();
            }

            if (stateTimer > maxStateDuration)
                ReturnToPreviousState();
        }

        public override void ExitState()
        {
            currentAbility?.StopAbility();
            _enemyStateMachine.Agent.isStopped = false;
        }

        private void StartNextAbility()
        {
            currentAbility = allAbilities.FirstOrDefault(a => a.IsReady && !a.IsActive);
            currentAbility?.StartAbility();
        }
      
        private void ReturnToPreviousState()
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
