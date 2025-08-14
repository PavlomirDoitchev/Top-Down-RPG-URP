using Assets.Scripts.Player;
using UnityEngine;
using TMPro;
using Assets.Scripts.Utility.UI;
using UnityEngine.UI;

public class UpdateHealthUI : MonoBehaviour, IObserver
{
    [SerializeField] Subject _playerStats;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI resourceText;
    [SerializeField] Image healthBarFillImage;
    [SerializeField] Image resourceBarFillImage;
    int currentHealth;
    int maxHealth;
    int currentResource;
    int maxResource;
    public void OnNotify()
    {
        currentHealth = _playerStats.GetComponent<PlayerStats>().GetCurrentHealth();
        maxHealth = _playerStats.GetComponent<PlayerStats>().GetMaxHealth();

        currentResource = _playerStats.GetComponent<PlayerStats>().GetCurrentResource();
        maxResource = _playerStats.GetComponent<PlayerStats>().GetMaxResource();

        float percantageOfMaxHealth = ((float)currentHealth / (float)maxHealth) * 100;
        float percantageOfMaxResource = ((float)currentResource / (float)maxResource) * 100;
        if (percantageOfMaxResource < 0)
        {
            percantageOfMaxResource = 0; 
        }
        if (float.IsNaN(percantageOfMaxResource))
        {
            percantageOfMaxResource = 100f;
        }
        if (percantageOfMaxHealth < 0)
        {
            percantageOfMaxHealth = 0; 
        }
        if (float.IsNaN(percantageOfMaxHealth))
        {
            percantageOfMaxHealth = 100f;
        }
       
        resourceText.text = $"{currentResource}/{percantageOfMaxResource:F0}%";
        healthText.text = $"{currentHealth}/{percantageOfMaxHealth:F0}%";
        healthBarFillImage.fillAmount = percantageOfMaxHealth / 100f;
        resourceBarFillImage.fillAmount = percantageOfMaxResource / 100f;
    }
    private void OnEnable()
    {
        _playerStats.AddObserver(this);
    }
    private void OnDisable()
    {
        _playerStats.RemoveObserver(this);
    }


}
