using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void DealDamage(int damage) 
    {
        if (currentHealth == 0) return;
        currentHealth -= damage;
        
    }
}
