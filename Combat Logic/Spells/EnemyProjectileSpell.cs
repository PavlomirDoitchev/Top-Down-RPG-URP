using UnityEngine;
namespace Assets.Scripts.Combat_Logic
{
    public class EnemyProjectileSpell : MonoBehaviour, IProjectile
    {
        [SerializeField] private ProjectileData projectileData;
        [SerializeField] private StatusEffectData effectData;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private LayerMask selfDestroyLayer;
        private Transform target;
        private Rigidbody rb;

        [Header("Spell Stats")]
        [SerializeField] private float offSet;
        [SerializeField] private float timer;
        Vector3 direction;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            timer = projectileData.lifeTime;
        }
        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                gameObject.SetActive(false);
                timer = projectileData.lifeTime;
            }
        }
        private void FixedUpdate()
        {
            rb.MovePosition(transform.position + direction * projectileData.speed * Time.fixedDeltaTime);
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground")
                || other.gameObject.layer == LayerMask.NameToLayer("Default"))
                gameObject.SetActive(false);

            if (effectData != null && other.TryGetComponent<IEffectable>(out var effectable)
                && other.gameObject.layer == LayerMask.NameToLayer("MyOutlines"))
            {
                effectable.ApplyEffect(effectData);
            }
            if (other.TryGetComponent<IDamagable>(out var damagable) && other.gameObject.layer == LayerMask.NameToLayer("MyOutlines"))
            {
                damagable.TakeDamage(projectileData.damage, true);
                projectileData.damageNumberPrefab.Spawn(other.transform.position, projectileData.damage);
                gameObject.SetActive(false);
            }
        }
        public void Initialize(Transform newTarget)
        {
            target = newTarget;
            Vector3 aimModifier = new Vector3(
                Random.Range(-projectileData.aimNoise_X, projectileData.aimNoise_X),  
                Random.Range(-projectileData.aimNoise_Y, projectileData.aimNoise_Y),  
                0);

            Vector3 aimPoint = AimLocation(target);
            direction = (aimPoint + aimModifier - transform.position).normalized;

            transform.rotation = Quaternion.LookRotation(direction);
        }
        public void Initialize(Transform newTarget, Vector3 spreadDirection) 
        {
            target = newTarget;
            Vector3 aimPoint = AimLocation(target);

            direction = (aimPoint - transform.position + spreadDirection).normalized;

            transform.rotation = Quaternion.LookRotation(direction);
        }
        public Vector3 AimLocation(Transform target)
        {
            if (target.TryGetComponent<CharacterController>(out var controller))
            {
                Debug.DrawLine(transform.position, target.transform.position + Vector3.up * controller.height * offSet, Color.red);
                return target.transform.position + Vector3.up * controller.height * offSet;
            }
            return target.transform.position + Vector3.up * 1f;
        }


    }
}
