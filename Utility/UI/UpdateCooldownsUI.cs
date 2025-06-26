using Assets.Scripts.Player;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Utility.UI
{
    public class UpdateCooldownsUI : MonoBehaviour, IObserver
    {
        [SerializeField] private List<Skills> trackedSkills;
        [SerializeField] private TextMeshProUGUI[] cooldownTexts;

        private void Start()
        {
            for (int i = 0; i < trackedSkills.Count; i++)
            {
                trackedSkills[i].AddObserver(this);
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < trackedSkills.Count; i++)
            {
                trackedSkills[i].RemoveObserver(this);
            }
        }

        public void OnNotify()
        {
            UpdateCooldownText();
        }

        private void Update()
        {
            UpdateCooldownText();
        }

        private void UpdateCooldownText()
        {
            for (int i = 0; i < trackedSkills.Count && i < cooldownTexts.Length; i++)
            {
                float timeLeft = Mathf.Max(0, trackedSkills[i].GetCooldownTimer());
                cooldownTexts[i].text = timeLeft > 0 ? $"{timeLeft:F1}s" : "";
            }
        }
    }
}
