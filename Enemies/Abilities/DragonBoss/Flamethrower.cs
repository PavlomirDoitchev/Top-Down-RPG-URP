using Assets.Scripts.Enemies.Abilities.Interfaces;
using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.Enemies.Abilities
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyFlamethrower : SpecialAbilityBase
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
        [SerializeField] private int priority = 1;

        private EnemyStateMachine stateMachine;
        private float durationTimer;
        private SphereCollider sphereCollider;

        private readonly Dictionary<Collider, float> lastHitTimes = new();

        public override string AbilityName => abilityName;
        public override float Duration => duration;
        public override int Priority => priority;
        public override bool IsActive { get; protected set; }

        private void Awake()
        {
            stateMachine = GetComponentInParent<EnemyStateMachine>();
            sphereCollider = GetComponent<SphereCollider>();
            ConfigureCollider();
        }

        private void OnValidate()
        {
            if (sphereCollider == null)
                sphereCollider = GetComponent<SphereCollider>();

            ConfigureCollider();
        }

        private void ConfigureCollider()
        {
            if (sphereCollider == null) return;
            sphereCollider.isTrigger = true;
            sphereCollider.radius = maxRange;
            sphereCollider.center = new Vector3(0f, 1f, 0f);
        }

        public override void StartAbility()
        {
            if (!IsReady) return;

            IsActive = true;
            durationTimer = Duration;

            ResetCooldown();

            if (flameVFX != null && !flameVFX.isPlaying)
                flameVFX.Play();

            stateMachine.Animator.CrossFadeInFixedTime(stateMachine.SpecialAbilityAnimationName, 0.1f);
        }

        public override void TickCooldown(float deltaTime)
        {
            base.TickCooldown(deltaTime);

            if (!IsActive) return;

            durationTimer -= deltaTime;
            if (durationTimer <= 0f)
            {
                StopAbility();
            }
        }

        public override void StopAbility()
        {
            IsActive = false;

            if (flameVFX != null && flameVFX.isPlaying)
                flameVFX.Stop();

            lastHitTimes.Clear();
        }

        public override bool ShouldRotateToPlayer() => rotateToPlayer;

        private void OnTriggerStay(Collider other)
        {
            if (!IsActive) return;

            Vector3 origin = transform.position; /*+ Vector3.up*/
            Vector3 forward = transform.forward;
            Vector3 dirToTarget = (other.transform.position - origin).normalized;

            float angle = Vector3.Angle(forward, dirToTarget);
            if (angle > coneAngle * 0.5f) return;

            if (other.TryGetComponent<IDamagable>(out var damagable) && other.gameObject.layer == LayerMask.NameToLayer("MyOutlines"))
            {
                if (!lastHitTimes.TryGetValue(other, out float lastHit) || Time.time - lastHit >= tickInterval)
                {
                    damagable.TakeDamage(baseDamage, false);

                    if (damageNumberPrefab != null)
                        damageNumberPrefab.Spawn(other.transform.position + Vector3.up * 2f, baseDamage);

                    if (burnEffect != null && other.TryGetComponent<IEffectable>(out var effectable))
                        effectable.ApplyEffect(burnEffect);

                    lastHitTimes[other] = Time.time;
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!IsActive) return;

            Vector3 origin = transform.position + Vector3.up;
            Vector3 forward = transform.forward;

            Gizmos.color = new Color(1f, 0.5f, 0f, 0.15f);
            Gizmos.DrawWireSphere(origin, maxRange);

            Handles.color = new Color(1f, 0.5f, 0f, 0.2f);
            Handles.DrawSolidArc(
                origin,
                Vector3.up,
                Quaternion.Euler(0, -coneAngle * 0.5f, 0) * forward,
                coneAngle,
                maxRange
            );

            Gizmos.color = new Color(1f, 0.5f, 0f, 0.6f);
            Vector3 rightBoundary = Quaternion.Euler(0, coneAngle * 0.5f, 0) * forward;
            Vector3 leftBoundary = Quaternion.Euler(0, -coneAngle * 0.5f, 0) * forward;
            Gizmos.DrawLine(origin, origin + rightBoundary * maxRange);
            Gizmos.DrawLine(origin, origin + leftBoundary * maxRange);

            GUIStyle labelStyle = new GUIStyle
            {
                normal = new GUIStyleState { textColor = Color.red },
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            //Vector3 labelPos = origin + Vector3.up * 2f;
            //Handles.Label(labelPos, $"{abilityName}\n{coneAngle:0}° / {maxRange:0.0}m", labelStyle);
        }
#endif
    }
}
