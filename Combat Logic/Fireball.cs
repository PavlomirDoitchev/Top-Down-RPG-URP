using UnityEngine;
namespace Assets.Scripts.Combat_Logic
{
    public class Fireball : MonoBehaviour, IProjectile
    {
        public float _speed = 10f;
        public int damage = 20;
        public float _lifeTime = 5f;
        [SerializeField] private Transform target;
        private Rigidbody rb;
        Vector3 direction;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            Destroy(gameObject, _lifeTime);
            transform.LookAt(AimLocation(target));
            direction = (AimLocation(target) - transform.position).normalized;
        }
        private void Update()
        {
            rb.MovePosition(transform.position + direction * _speed * Time.deltaTime);
            
        }
        
        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Default")
                || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
                Destroy(gameObject);

            if (other.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.TakeDamage(damage);
                Destroy(gameObject);
            }


        }
    
    
        public Vector3 AimLocation(Transform target)
        {
            return target.transform.position;
        }

        
    }
}
