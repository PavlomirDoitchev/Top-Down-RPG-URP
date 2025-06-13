using UnityEngine;
namespace Assets.Scripts.Combat_Logic
{
	public class EnemyProjectileSpell : MonoBehaviour, IProjectile
	{
		[SerializeField] private ProjectileData projectileData;
		[SerializeField] private StatusEffectData effectData;
		[SerializeField] private LayerMask targetLayer;
		[SerializeField] private LayerMask selfDestroyLayer;
		[SerializeField] private GameObject spellHitPrefab;
		private Transform target;
		private Rigidbody rb;

		[Header("Spell Stats")]
		[SerializeField] private float aimModifier_X = 0.5f;
		[SerializeField] private float aimModifier_Y = 0.5f;
		[SerializeField] private float offSet;
		[SerializeField] private float timer;
		Vector3 direction;
		Vector3 aimModifier;
		void Start()
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

		}
		private void FixedUpdate()
		{

			rb.MovePosition(transform.position + direction * projectileData.speed * Time.fixedDeltaTime);
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Ground")
				|| other.gameObject.layer == LayerMask.NameToLayer("Default"))
			{
				Instantiate(spellHitPrefab, this.transform.position, Quaternion.identity);
				gameObject.SetActive(false);
			}

			if (effectData != null && other.TryGetComponent<IEffectable>(out var effectable)
				&& other.gameObject.layer == LayerMask.NameToLayer("MyOutlines"))
			{
				effectable.ApplyEffect(effectData);
			}

			if (other.TryGetComponent<IDamagable>(out var damagable) && other.gameObject.layer == LayerMask.NameToLayer("MyOutlines"))
			{
				Instantiate(spellHitPrefab, this.transform.position, Quaternion.identity);
				damagable.TakeDamage(projectileData.damage, true);
				projectileData.damageNumberPrefab.Spawn(other.transform.position, projectileData.damage);
				gameObject.SetActive(false);
			}
		}
		public void Initialize(Transform newTarget)
		{
			target = newTarget;
			if (target == null)
			{
				Debug.Log($"{target} is null!");
			}
			if (target.TryGetComponent<CharacterController>(out var controller)
				&& controller.velocity.x != 0 || controller.velocity.z != 0)
			{
				aimModifier = new Vector3(
				Random.Range(-aimModifier_X, aimModifier_X),
				0,
				Random.Range(-aimModifier_Y, aimModifier_Y));

			}
			else
			{
				aimModifier = Vector3.zero;
			}

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
