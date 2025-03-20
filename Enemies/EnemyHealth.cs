using UnityEngine;
using Assets.Scripts.State_Machine.Player;

public class EnemyHealth : MonoBehaviour
{
    int damage = 15;
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    //PlayerStats playerStats;
    private void Start()
    {
        currentHealth = maxHealth;
        //if (playerStats == null)
        //{
        //    GameObject player = GameObject.FindWithTag("Player");
        //    if (player != null)
        //    {
        //        playerStats = player.GetComponent<PlayerStats>();
        //    }
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats.Instance.PlayerTakeDamage(DealDamage());
            //playerStats.PlayerTakeDamage(DealDamage());
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
