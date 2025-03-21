using UnityEngine;
using Assets.Scripts.State_Machine.Player;

public class EnemyHealth : MonoBehaviour
{
    int damage = 15;
    int coolDown = 1;
    float timer = 0;
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    Vector3 force;
    PlayerStats playerStats;
    private void Start()
    {
        playerStats = PlayerStats.Instance;
        force = transform.position - playerStats.transform.position;
        currentHealth = maxHealth;


    }
    private void Update()
    {
        timer -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerStats.PlayerTakeDamage(DealDamage());
            if (timer <= 0)
            {
                playerStats.playerStateMachine.ForceReceiver.AddForce((other.transform.position - this.transform.position).normalized * 15);
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
        playerStats.GainXP(1);
        //playerStats.GainXP(1);
        //Debug.Log(gameObject.name + " has died!");
        //Destroy(gameObject);
    }
}
