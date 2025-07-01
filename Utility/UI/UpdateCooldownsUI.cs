using Assets.Scripts.Player;
using Assets.Scripts.Player.Abilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utility.UI
{
    public class UpdateCooldownsUI : MonoBehaviour, IObserver
    {
        [SerializeField] private List<Skills> trackedSkills;
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private TextMeshProUGUI[] cooldownTexts;
        [SerializeField] private TextMeshProUGUI[] chargeTexts;
        [SerializeField] private Image[] skillIcons;
        [SerializeField] private Image[] empoweredSkillIcons;

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
        private void OnEnable()
        {
            for (int i = 0; i < trackedSkills.Count; i++)
            {
                trackedSkills[i].AddObserver(this);
            }
            playerStats.AddObserver(this);
        }
        private void OnDisable()
        {
            for (int i = 0; i < trackedSkills.Count; i++)
            {
                trackedSkills[i].RemoveObserver(this);
            }
            playerStats.RemoveObserver(this);
        }
        public void OnNotify()
        {
            UpdateCooldownText();
            UpdateChargesText();
            EmpoweredIcons();
        }
        public void EmpoweredIcons()
        {
            for (int i = 0; i < trackedSkills.Count && i < empoweredSkillIcons.Length; i++)
            {
                if (playerStats.GetCurrentResource() >= 50)
                {
                    empoweredSkillIcons[i].color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * 2, 1));
                }
                else
                {
                    empoweredSkillIcons[i].color = Color.white;
                }
            }

        }
      
        private void UpdateChargesText()
        {
            for (int i = 0; i < trackedSkills.Count && i < cooldownTexts.Length; i++)
            {
                if (trackedSkills[i] is ICharges)
                {
                    int chargeCount = trackedSkills[i] is ICharges chargesSkill ? chargesSkill.GetChargeCount() : 0;

                    chargeTexts[i].text = chargeCount > 0 ? chargeCount.ToString() : "";
                }
                else
                {
                    chargeTexts[i].text = "";

                }
            }
        }
        private void UpdateCooldownText()
        {

            for (int i = 0; i < trackedSkills.Count && i < cooldownTexts.Length; i++)
            {
                float timeLeft = Mathf.Max(0, trackedSkills[i].GetCooldownTimer());
                if (trackedSkills[i] is ICharges chargesSkill
                    && chargesSkill.GetChargeCount() > 0)
                {
                    float remainingDuration = Mathf.Max(0, chargesSkill.GetRemainingTime());
                    cooldownTexts[i].color = remainingDuration > 0 && chargesSkill.GetChargeCount() > 0 ? Color.green : Color.white;
                    cooldownTexts[i].text = remainingDuration > 0 ? $"{chargesSkill.GetRemainingTime():F0}" : "";
                    if (chargesSkill.GetRemainingTime() <= 0f)
                    {
                        skillIcons[i].color = timeLeft > 0 ? Color.grey : Color.white;
                        cooldownTexts[i].text = timeLeft > 0 ? $"{timeLeft:F0}" : "";
                    }
                }

                else
                {
                    skillIcons[i].color = timeLeft > 0 ? Color.grey : Color.white;
                    cooldownTexts[i].text = timeLeft > 0 ? $"{timeLeft:F0}" : "";
                }
            }
        }

    }
}