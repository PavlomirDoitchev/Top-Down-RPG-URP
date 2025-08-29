using Assets.Scripts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utility.UI
{
    public class CharacterInformationUI : MonoBehaviour, IObserver
    {
        [SerializeField] Subject _playerStats;
        
        [SerializeField] TextMeshProUGUI levelText;
        //[SerializeField] TextMeshProUGUI xpText;
        [SerializeField] Image xpProgressBar;
        private int currentLevel;
        private int currentExperience;
        public void OnNotify()
        {
            currentLevel = _playerStats.GetComponent<PlayerStats>().GetCurrentLevel();
            currentExperience = _playerStats.GetComponent<PlayerStats>().GetCurrentXP();
            levelText.text = $"Level {currentLevel.ToString()}";
            //xpText.text = currentExperience.ToString();
            int xpToNext = _playerStats.GetComponent<PlayerStats>().GetXPToNextLevel();

            float progress = 0f;
            if (xpToNext > 0)
            {
                progress = Mathf.Clamp01((float)currentExperience / xpToNext);
            }

            xpProgressBar.fillAmount = progress;
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
}
