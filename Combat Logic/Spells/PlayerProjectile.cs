using Assets.Scripts.Player;
using UnityEngine;
namespace Assets.Scripts.Combat_Logic
{
    public class PlayerProjectile : MonoBehaviour
    {
        [SerializeField] private ProjectileData projectileData;
        [SerializeField] private StatusEffectData effectData;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private LayerMask selfDestroyLayer;
        [SerializeField] private GameObject spellHitPrefab;
        private Transform target;
        private Rigidbody rb;

        [Header("Spell Stats")]
        [SerializeField] private float offSet;
        [SerializeField] private float timer;
        Vector3 direction;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
            
            timer = projectileData.lifeTime;
        }
        private void OnDisable()
        {
            this.transform.position = Vector3.zero;
            timer = projectileData.lifeTime;
        }
        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                gameObject.SetActive(false);
            }
            transform.position += direction * projectileData.speed * Time.deltaTime;

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground")
                || other.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                if (spellHitPrefab != null)
                    Instantiate(spellHitPrefab, this.transform.position, Quaternion.identity);
                Debug.Log($"Projectile hit {other.gameObject.name}");
                gameObject.SetActive(false);
            }
           
            else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (effectData != null && other.TryGetComponent<IEffectable>(out var effectable)) 
                {
                    effectable.ApplyEffect(effectData);
                }
                if (projectileData != null && other.TryGetComponent<IDamagable>(out var damagable)) 
                {
                    if (spellHitPrefab != null)
                        Instantiate(spellHitPrefab, this.transform.position, Quaternion.identity); //Remove later, add to a pool
                    damagable.TakeDamage(projectileData.damage, false);
                    projectileData.damageNumberPrefab.Spawn(other.transform.position, projectileData.damage);
                }
                Debug.Log($"Projectile hit {other.gameObject.name}");
                gameObject.SetActive(false);
            }

        }
        public void Init() 
        {
            Vector3 aimPoint = SkillManager.Instance.AimSpell();
            direction = (aimPoint - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
