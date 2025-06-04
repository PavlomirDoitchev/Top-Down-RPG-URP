using UnityEngine;
using Assets.Scripts.Player;
using Assets.Scripts.Combat_Logic;
using Assets.Scripts.State_Machine.Enemy_State_Machine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    private readonly int damage = 15;
    private readonly int coolDown = 1;
    float timer = 0;
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int xpReward = 1;
    PlayerManager playerManager;
    [SerializeField] EnemyStateMachine enemyStateMachine;
    private void Start()
    {
        currentHealth = maxHealth;
        playerManager = PlayerManager.Instance;

    }
    private void Update()
    {
        timer -= Time.deltaTime;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && this.gameObject.layer != 0)
    //    {
    //        var playerStats = playerManager.PlayerStateMachine.PlayerStats;
    //        playerStats.TakeDamage(DealDamage());
    //        if (playerStats.GetResourceType() == CharacterLevelSO.ResourceType.Rage)
    //            playerStats.RegainResource(5);
    //        //if (timer <= 0)
    //        //{
    //        //    playerManager.PlayerStateMachine.ForceReceiver.AddForce((other.transform.position - this.transform.position).normalized * 15);
    //        //    timer = coolDown;
    //        //}
    //    }
    //}
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
        //currentHealth = maxHealth;
        //Debug.Log(gameObject.name + " has died!");
        playerManager.PlayerStateMachine.PlayerStats.GainXP(xpReward);

        enemyStateMachine.ChangeState(new OrkDeathState(enemyStateMachine));
        //playerStats.GainXP(1);
        //Destroy(gameObject);
    }
}
