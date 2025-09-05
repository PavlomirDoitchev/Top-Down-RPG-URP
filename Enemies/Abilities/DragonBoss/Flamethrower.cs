using Assets.Scripts.Enemies.Abilities.Interfaces;
using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

namespace Assets.Scripts.Enemies.Abilities
{
    public class EnemyFlamethrower : MonoBehaviour, ISpecialAbility
    {
        [Header("Ability Info")]
        [SerializeField] private string abilityName = "Flamethrower";
        [SerializeField] private float duration = 3f;
        [SerializeField] private bool rotateToPlayer = true;

        [Header("Flamethrower Settings")]
        [SerializeField] private float tickInterval = 0.25f;
        [SerializeField] private float maxRange = 6f;
        [SerializeField] private float coneAngle = 60f;
        [SerializeField] private int baseDamage = 15;
        [SerializeField] private StatusEffectData burnEffect;
        [SerializeField] private ParticleSystem flameVFX;
        [SerializeField] private DamageNumber damageNumberPrefab;

        [Header("Ability Settings")]
        [SerializeField] private float cooldownDuration = 8f;
        [SerializeField] private int priority = 1;

        private EnemyStateMachine stateMachine;
        private float durationTimer;
        private float tickTimer;
        private bool isActive;

        private readonly Cooldown cooldown = new();
        private readonly List<Collider> hitCache = new();

        // ISpecialAbility Implementation
        public string AbilityName => abilityName;
        public float Duration => duration;
        public bool IsActive => isActive;
        public bool IsReady => cooldown.IsReady;
        public int Priority => priority;

        private void Awake()
        {
            stateMachine = GetComponentInParent<EnemyStateMachine>();
        }
      
        public void StartAbility()
        {
            if (!IsReady) return;

            isActive = true;
            durationTimer = duration;
            tickTimer = 0f;
            cooldown.Start(cooldownDuration);

            if (flameVFX != null && !flameVFX.isPlaying) flameVFX.Play();

            // Play cast animation
            stateMachine.Animator.CrossFadeInFixedTime(stateMachine.SpecialAbilityAnimationName, 0.1f);
        }

        public void StopAbility()
        {
            isActive = false;
            if (flameVFX != null && flameVFX.isPlaying) flameVFX.Stop();
        }

        public void TickCooldown(float deltaTime)
        {
            cooldown.Tick(deltaTime);
            if (!isActive) return;

            durationTimer -= deltaTime;
            tickTimer -= deltaTime;

            if (tickTimer <= 0f)
            {
                ApplyDamageCone();
                tickTimer = tickInterval;
            }

            if (durationTimer <= 0f)
            {
                StopAbility();
            }
        }

        public bool ShouldRotateToPlayer() => rotateToPlayer;

        private void ApplyDamageCone()
        {
            Vector3 origin = transform.position + Vector3.up * 1.2f;
            Vector3 forward = transform.forward;

            hitCache.Clear();
            Collider[] hits = Physics.OverlapSphere(origin, maxRange, 1 << LayerMask.NameToLayer("MyOutlines"));

            foreach (Collider hit in hits)
            {
                Vector3 dirToTarget = (hit.transform.position - origin).normalized;
                float angle = Vector3.Angle(forward, dirToTarget);

                if (angle <= coneAngle * 0.5f)
                {
                    if (hit.TryGetComponent<IDamagable>(out var damagable))
                    {
                        int finalDamage = baseDamage;

                      
                        hitCache.Add(hit);

                        damagable.TakeDamage(finalDamage, false);

                        if (damageNumberPrefab != null)
                            damageNumberPrefab.Spawn(hit.transform.position + Vector3.up * 2f, finalDamage);

                        if (burnEffect != null && hit.TryGetComponent<IEffectable>(out var effectable))
                            effectable.ApplyEffect(burnEffect);
                        hitCache.Clear();
                    }
                }
            }
            
        }
        private void OnDrawGizmos()
        {
            if (!isActive) return;

            Vector3 origin = transform.position + Vector3.up * 1.2f;
            Vector3 forward = transform.forward;

            Gizmos.color = new Color(1f, 0.5f, 0f, 0.25f);
            Gizmos.DrawWireSphere(origin, maxRange);

            Vector3 rightBoundary = Quaternion.Euler(0, coneAngle * 0.5f, 0) * forward;
            Vector3 leftBoundary = Quaternion.Euler(0, -coneAngle * 0.5f, 0) * forward;

            Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
            Gizmos.DrawLine(origin, origin + rightBoundary * maxRange);
            Gizmos.DrawLine(origin, origin + leftBoundary * maxRange);
        }
    }
}
