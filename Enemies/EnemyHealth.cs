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
    private void Awake()
    {
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
            PlayerStats.Instance.PlayerTakeDamage(DealDamage());
            if (timer <= 0)
            {
                force = transform.position - PlayerStats.Instance.transform.position;
                PlayerStats.Instance.playerStateMachine.ForceReceiver.AddForce((other.transform.position - this.transform.position).normalized * 15);
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
        PlayerStats.Instance.GainXP(1);
        //playerStats.GainXP(1);
        //Debug.Log(gameObject.name + " has died!");
        //Destroy(gameObject);
    }
}
