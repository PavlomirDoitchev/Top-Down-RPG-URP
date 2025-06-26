using Assets.Scripts.Player;
using UnityEngine;
using TMPro;
using Assets.Scripts.Utility.UI;

public class UpdateHealthUI : MonoBehaviour, IObserver
{
    [SerializeField] Subject _playerStats;
    [SerializeField] TextMeshProUGUI healthText;

    public void OnNotify()
    {
        //Debug.Log("UpdateUI OnNotify called");
        healthText.text = _playerStats.GetComponent<PlayerStats>().GetCurrentHealth().ToString();
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
