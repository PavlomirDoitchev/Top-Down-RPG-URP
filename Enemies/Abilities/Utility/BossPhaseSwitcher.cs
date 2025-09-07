using Assets.Scripts.Enemies.Abilities.Interfaces;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies.Utility
{
    [System.Serializable]
    public class BossPhaseConfig
    {
        public string phaseName;
        [Tooltip("Abilities that should be active during this phase (must implement ISpecialAbility).")]
        public MonoBehaviour[] abilities;  
        public string transitionAnim;
        [Tooltip("How long to wait before enabling abilities (e.g., for a roar/transition anim).")]
        public float transitionDelay = 1f;
    }

    public class BossPhaseSwitcher : MonoBehaviour
    {
        [SerializeField] private EnemyStateMachine stateMachine;
        [SerializeField] private bool shouldBeUntargetableDuringPhase = false;
        [SerializeField] private BossPhaseConfig[] phases;

        private int currentPhase = 0;

        private void Awake()
        {
            if (stateMachine == null)
                stateMachine = GetComponent<EnemyStateMachine>();

            if (stateMachine != null)
                stateMachine.OnPhaseThresholdReached += SwitchPhase;
        }

        private void OnDestroy()
        {
            if (stateMachine != null)
                stateMachine.OnPhaseThresholdReached -= SwitchPhase;
        }

        private void Start()
        {
            // wait a frame so all components have finished their Start/Awake
            StartCoroutine(SwitchPhaseNextFrame(1));
        }

        private IEnumerator SwitchPhaseNextFrame(int phase)
        {
            yield return null;
            SwitchPhase(phase);
        }

        private void SwitchPhase(int phase)
        {
            if (phase == currentPhase) return;
            if (phases == null || phases.Length < phase)
            {
                Debug.LogWarning($"BossPhaseSwitcher: invalid phase {phase}");
                return;
            }

            currentPhase = phase;
            var config = phases[phase - 1];

            Debug.Log($"Switching to Phase {phase}: {config.phaseName}");
           
            foreach (var ability in stateMachine.SpecialAbilities)
            {
                if (ability is MonoBehaviour mb)
                    mb.enabled = false;
                ability.StopAbility();
            }

            var newAbilities = new List<ISpecialAbility>();
            foreach (var mb in config.abilities)
            {
                if (mb is ISpecialAbility special)
                {
                    mb.enabled = true;
                    newAbilities.Add(special);
                }
            }

            stateMachine.SetActiveAbilities(newAbilities.ToArray());

        }

        private IEnumerator HandlePhaseTransition(BossPhaseConfig config)
        {
            if (shouldBeUntargetableDuringPhase)
                stateMachine.Agent.enabled = false;

            if (!string.IsNullOrEmpty(config.transitionAnim))
                stateMachine.Animator.CrossFadeInFixedTime(config.transitionAnim, 0.15f);

            yield return new WaitForSeconds(Mathf.Max(0.01f, config.transitionDelay));

            // Enable only the abilities for this phase
            if (config.abilities != null)
            {
                foreach (var mb in config.abilities)
                {
                    if (mb == null) continue;

                    if (mb is ISpecialAbility)
                    {
                        mb.enabled = true; 
                    }
                    else
                    {
                        Debug.LogWarning($"{mb.name} assigned in BossPhaseSwitcher is not an ISpecialAbility.");
                    }
                }
            }

            if (shouldBeUntargetableDuringPhase)
                stateMachine.Agent.enabled = true;
        }
    }
}

