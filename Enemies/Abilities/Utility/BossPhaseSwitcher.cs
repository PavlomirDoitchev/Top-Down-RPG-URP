using Assets.Scripts.Enemies.Abilities.Interfaces;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemies.Utility
{
    [System.Serializable]
    public class BossPhaseConfig
    {
        public string phaseName;
        public MonoBehaviour[] abilities;
        public string transitionAnim;
    }
    public class BossPhaseSwitcher : MonoBehaviour
    {
        [SerializeField] private EnemyStateMachine stateMachine;


        [SerializeField] bool shouldBeUntargetableDuringPhase = false;
        [SerializeField] private BossPhaseConfig[] phases;
        private int currentPhase = 0;

        private void Awake()
        {
            if (stateMachine == null)
                stateMachine = GetComponent<EnemyStateMachine>();

            stateMachine.OnPhaseThresholdReached += SwitchPhase;
        }

        private void OnDestroy()
        {
            if (stateMachine != null)
                stateMachine.OnPhaseThresholdReached -= SwitchPhase;
        }

        private void Start()
        {
            stateMachine.OnPhaseThresholdReached += SwitchPhase;
            StartCoroutine(SwitchPhaseNextFrame(1));
            
        }

        private void SwitchPhase(int phase)
        {
            if (phase == currentPhase) return;
            currentPhase = phase;

            Debug.Log($"Switching to Phase {phase}: {phases[phase - 1].phaseName}");

            foreach (var ability in stateMachine.GetComponentsInChildren<ISpecialAbility>())
                (ability as MonoBehaviour).enabled = false;

            foreach (var ability in phases[phase - 1].abilities)
                if (ability != null) ability.enabled = true;

            var anim = phases[phase - 1].transitionAnim;
            if (!string.IsNullOrEmpty(anim))
                stateMachine.Animator.CrossFadeInFixedTime(anim, 0.2f);
        }
        private void EnableAbilities(MonoBehaviour[] abilities)
        {
            foreach (var ability in abilities)
                if (ability != null)
                    ability.enabled = true;
        }

        private void DisableAllAbilities()
        {
            foreach (var ability in stateMachine.GetComponentsInChildren<ISpecialAbility>())
            {
                if (ability is MonoBehaviour mb)
                    mb.enabled = false;
            }
        }

        private void PlayAnimation(string animName)
        {
            if (!string.IsNullOrEmpty(animName))
                stateMachine.Animator.CrossFadeInFixedTime(animName, 0.1f);
        }
        private IEnumerator SwitchPhaseNextFrame(int phase)
        {
            yield return null; // wait 1 frame
            SwitchPhase(phase);
        }
    }
}
