using UnityEngine;
using Assets.Scripts.Player;
using Assets.Scripts.Combat_Logic;
using Assets.Scripts.State_Machine.Enemy_State_Machine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int xpReward = 1;
    [SerializeField][Range(0,1)] private float enrageThreshold;
    PlayerManager playerManager;
    [SerializeField] EnemyStateMachine enemyStateMachine;
    private void Start()
    {
        currentHealth = maxHealth;
        playerManager = PlayerManager.Instance;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= Mathf.RoundToInt(maxHealth * enrageThreshold) && enemyStateMachine.CanBecomeEnraged) 
        {
            enemyStateMachine.IsEnraged = true;
        }
        if (currentHealth <= 0)
        {
            enemyStateMachine.ShouldDie = true;
            playerManager.PlayerStateMachine.PlayerStats.GainXP(xpReward);
        }
    } 
}
