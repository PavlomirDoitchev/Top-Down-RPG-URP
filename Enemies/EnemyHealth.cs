using UnityEngine;
using Assets.Scripts.Player;

public class EnemyHealth : MonoBehaviour
{
    int damage = 15;
    int coolDown = 1;
    float timer = 0;
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    Vector3 force;
    PlayerManager playerManager;
    private void Awake()
    {
        currentHealth = maxHealth;
        playerManager = PlayerManager.Instance;

    }
    private void Update()
    {
        timer -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerStats = playerManager.playerStateMachine._PlayerStats;
            playerStats.PlayerTakeDamage(DealDamage());
            if (playerStats.GetResourceType() == CharacterLevelSO.ResourceType.Rage)
                playerStats.RegainResource(Mathf.RoundToInt(damage * 0.1f));
            if (timer <= 0)
            {
                force = transform.position - playerManager.transform.position;
                playerManager.playerStateMachine.ForceReceiver.AddForce((other.transform.position - this.transform.position).normalized * 15);
                timer = coolDown;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        Debug.Log(gameObject.name + " took " + damage + " damage!");
    }
    public int DealDamage()
    {
        return this.damage;
    }

    private void Die()
    {
        currentHealth = maxHealth;
        playerManager.playerStateMachine._PlayerStats.GainXP(1);
        //playerStats.GainXP(1);
        //Debug.Log(gameObject.name + " has died!");
        //Destroy(gameObject);
    }
}
