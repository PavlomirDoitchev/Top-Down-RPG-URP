using Assets.Scripts.Player;
using UnityEngine;
using Assets.Scripts.Combat_Logic;
public abstract class PlayerProjectile : MonoBehaviour
{
    public ProjectileData projectileData;
    public StatusEffectData effectData;
    public LayerMask targetLayer;
    public LayerMask selfDestroyLayer;
    public GameObject spellHitPrefab;
    public Transform target;
    public Rigidbody rb;


    [Header("Spell Stats")]
    public float offSet;
    public float timer;
    Vector3 direction;
    protected bool isChaining = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
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
        if (isChaining) return; 

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            gameObject.SetActive(false);
        }

        transform.position += (direction * projectileData.speed) * Time.deltaTime;

    }
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }
    public void Init()
    {
        Vector3 aimPoint = SkillManager.Instance.AimSpell();
        direction = (aimPoint - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}

