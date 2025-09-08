using Assets.Scripts.Enemies.Abilities.Interfaces;
using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;
using DamageNumbersPro;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using Assets.Scripts.State_Machine.Player_State_Machine;

namespace Assets.Scripts.Enemies.Abilities
{
    public class GroundLightningStrike : SpecialAbilityBase
    {
        [Header("Ability Info")]
        [SerializeField] private string abilityName = "Ground Lightning";
        [SerializeField] private float duration = 0.5f; // how long the meteor takes to hit
        [SerializeField] private bool rotateToPlayer = false;

        [Header("Explosion Settings")]
        [SerializeField] private float explosionRadius = 4f;
        [SerializeField] private int baseDamage = 40;
        [SerializeField] private float telegraphDelay = 1f; // how long the marker shows before impact
        [SerializeField] private GameObject groundMarkerPrefab;
        [SerializeField] private GameObject meteorPrefab;
        [SerializeField] private DamageNumber damageNumberPrefab;
        [SerializeField] private StatusEffectData effectData;

        [Header("Ability Settings")]
        [SerializeField] private int priority = 2;

        private EnemyStateMachine stateMachine;
        private bool isCasting;
        private Vector3 targetPosition;
        private GameObject currentMarker;

        public override string AbilityName => abilityName;
        public override float Duration => duration;
        public override int Priority => priority;
        public override bool IsActive { get; protected set; }

        private void Awake()
        {
            stateMachine = GetComponentInParent<EnemyStateMachine>();
        }

        public override void StartAbility()
        {
            if (!IsReady) return;

            IsActive = true;
            ResetCooldown();

            targetPosition = PlayerManager.Instance.PlayerStateMachine.transform.position;

            if (groundMarkerPrefab != null)
                currentMarker = Instantiate(groundMarkerPrefab, targetPosition, Quaternion.identity);
            stateMachine.Animator.CrossFadeInFixedTime(stateMachine.SpecialAbilityAnimationName, 0.1f);
            stateMachine.StartCoroutine(CastSpell());
        }

        private IEnumerator CastSpell()
        {
            isCasting = true;

            yield return new WaitForSeconds(telegraphDelay);

            if (meteorPrefab != null)
            {
                Vector3 spawnPosition = targetPosition;// meteor falls from height
                /*GameObject meteor =*/
                Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
                PlayerManager.Instance.PlayerStateMachine.CinemachineImpulseSource.GenerateImpulse();
            }

            ApplyExplosionDamage();

            if (currentMarker != null)
                Destroy(currentMarker);

            StopAbility();
        }

        private void ApplyExplosionDamage()
        {
            Collider[] hits = Physics.OverlapSphere(targetPosition, explosionRadius, 1 << LayerMask.NameToLayer("MyOutlines"));
            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent<IDamagable>(out var damagable))
                {
                    damagable.TakeDamage(baseDamage, false);

                    if (damageNumberPrefab != null)
                        damageNumberPrefab.Spawn(hit.transform.position + Vector3.up * 2f, baseDamage);
                    if (effectData != null && hit.TryGetComponent<IEffectable>(out var effectable))
                    {
                        effectable.ApplyEffect(effectData);
                        PlayerManager.Instance.PlayerStateMachine.ChangeState(new PlayerStunnedState(PlayerManager.Instance.PlayerStateMachine, effectData.DOTDuration));
                    }
                }
            }
        }

        public override void TickCooldown(float deltaTime)
        {
            base.TickCooldown(deltaTime);
        }

        public override void StopAbility()
        {
            IsActive = false;
            isCasting = false;
        }

        public override bool ShouldRotateToPlayer() => rotateToPlayer;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetPosition, explosionRadius);



        }
    }
}
//using Assets.Scripts.Enemies.Abilities.Interfaces;
//using Assets.Scripts.Combat_Logic;
//using Assets.Scripts.Player;
//using System.Collections;
//using UnityEngine;
//using DamageNumbersPro;
//using Assets.Scripts.State_Machine.Enemy_State_Machine;

//namespace Assets.Scripts.Enemies.Abilities
//{
//    public class EnemyGroundExplosion : SpecialAbilityBase
//    {
//        [Header("Ability Info")]
//        [SerializeField] private string abilityName = "Ground Explosion";
//        [SerializeField] private float duration = 0.5f; // how long the meteor takes to hit
//        [SerializeField] private bool rotateToPlayer = false;

//        [Header("Explosion Settings")]
//        [SerializeField] private float explosionRadius = 4f;
//        [SerializeField] private int baseDamage = 40;
//        [SerializeField] private float telegraphDelay = 1f; // how long the marker shows before impact
//        [SerializeField] private GameObject groundMarkerObject;
//        [SerializeField] private GameObject meteorObject;
//        [SerializeField] private DamageNumber damageNumberPrefab;

//        [Header("Ability Settings")]
//        [SerializeField] private int priority = 2;

//        private EnemyStateMachine stateMachine;
//        private bool isCasting;
//        private Vector3 targetPosition;

//        public override string AbilityName => abilityName;
//        public override float Duration => duration;
//        public override int Priority => priority;
//        public override bool IsActive { get; protected set; }

//        private void Awake()
//        {
//            stateMachine = GetComponentInParent<EnemyStateMachine>();
//        }

//        public override void StartAbility()
//        {
//            if (!IsReady) return;

//            IsActive = true;
//            ResetCooldown();

//            // Target the player's current position
//            targetPosition = PlayerManager.Instance.PlayerStateMachine.transform.position;

//            // Activate telegraph marker
//            if (groundMarkerObject != null)
//            {
//                ActivateObject(groundMarkerObject, targetPosition);
//            }

//            stateMachine.Animator.CrossFadeInFixedTime(stateMachine.SpecialAbilityAnimationName, 0.1f);
//            stateMachine.StartCoroutine(CastMeteor());
//        }

//        private IEnumerator CastMeteor()
//        {
//            isCasting = true;

//            // Wait for the telegraph delay
//            yield return new WaitForSeconds(telegraphDelay);

//            // Activate meteor object at player's position
//            if (meteorObject != null)
//            {
//                ActivateObject(meteorObject, targetPosition);
//            }

//            ApplyExplosionDamage();

//            // Deactivate telegraph marker
//            if (groundMarkerObject != null)
//                DeactivateObject(groundMarkerObject);

//            StopAbility();
//        }

//        private void ApplyExplosionDamage()
//        {
//            Collider[] hits = Physics.OverlapSphere(targetPosition, explosionRadius, 1 << LayerMask.NameToLayer("MyOutlines"));
//            foreach (Collider hit in hits)
//            {
//                if (hit.TryGetComponent<IDamagable>(out var damagable))
//                {
//                    damagable.TakeDamage(baseDamage, false);

//                    if (damageNumberPrefab != null)
//                        damageNumberPrefab.Spawn(hit.transform.position + Vector3.up * 2f, baseDamage);
//                }
//            }
//        }

//        private void ActivateObject(GameObject obj, Vector3 worldPosition)
//        {
//            obj.transform.position = worldPosition;
//            obj.SetActive(true);         // Activate first
//            obj.transform.SetParent(null); // Then detach from enemy

//            // Ensure child particle systems play
//            foreach (var ps in obj.GetComponentsInChildren<ParticleSystem>())
//                ps.Play(true);
//        }
//        private void DeactivateObject(GameObject obj)
//        {
//            obj.SetActive(false);
//            obj.transform.SetParent(stateMachine.transform);
//        }

//        public override void TickCooldown(float deltaTime)
//        {
//            base.TickCooldown(deltaTime);
//        }

//        public override void StopAbility()
//        {
//            IsActive = false;
//            isCasting = false;

//            // Ensure both objects are deactivated in case ability is stopped early
//            if (groundMarkerObject != null) DeactivateObject(groundMarkerObject);
//            if (meteorObject != null) DeactivateObject(meteorObject);
//        }

//        public override bool ShouldRotateToPlayer() => rotateToPlayer;

//        private void OnDrawGizmos()
//        {
//            if (!IsActive) return;
//            Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.25f);
//            Gizmos.DrawWireSphere(targetPosition, explosionRadius);
//        }
//    }
//}

