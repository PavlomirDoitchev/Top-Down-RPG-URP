using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Player;
using Assets.Scripts.Utility.UI;
using Unity.VisualScripting;

public class StatusEffectUIManager : MonoBehaviour, IObserver
{
    [SerializeField] private Subject playerStatsSubject;
    [SerializeField] private List<StatusEffectSlot> slots;
    
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = playerStatsSubject.GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        playerStatsSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        playerStatsSubject.RemoveObserver(this);
    }

    public void OnNotify()
    {
        UpdateEffectSlots();
    }

    private void UpdateEffectSlots()
    {
        // Clear all slots
        foreach (var slot in slots)
            slot.ClearSlot();

        // Get active effects
        Dictionary<StatusEffectData.StatusEffectType, PlayerStats.ActiveEffect> activeEffects = playerStats.GetActiveEffects();

        int index = 0;
        foreach (var effect in activeEffects.Values)
        {
            if (index >= slots.Count)
                break;

            Sprite icon = effect.Data.effectIcon;
            int stacks = effect.StackCount;

            if (icon != null)
            {
                slots[index].SetSlot(icon, stacks, effect);
                slots[index].OnNotify();
            }
            index++;
        }
    }
}
