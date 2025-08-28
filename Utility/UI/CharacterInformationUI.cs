using Assets.Scripts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Utility.UI
{
    public class CharacterInformationUI : IObserver
    {
        [SerializeField] Subject _playerStats;
        [SerializeField] TextMeshProUGUI levelText;
        [SerializeField] TextMeshProUGUI XPText;
        private int currentLevel;
        private int currentExperience;
        public void OnNotify()
        {
            currentLevel = _playerStats.GetComponent<PlayerStats>().GetCurrentLevel();
            currentExperience = _playerStats.GetComponent<PlayerStats>().GetCurrentXP();
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
