using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Assets.Scripts.Player.PlayerStats;
using Assets.Scripts.Utility.UI;
using Assets.Scripts.Player;

public class StatusEffectSlot : MonoBehaviour, IObserver
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI stackText;
    [SerializeField] private TextMeshProUGUI durationText;
    private ActiveEffect _activeEffect;
   
    public void SetSlot(Sprite icon, int stackCount, ActiveEffect effect)
    {
        _activeEffect = effect;

        iconImage.sprite = icon;
        iconImage.enabled = true;

        stackText.text = stackCount > 1 ? stackCount.ToString() : "";
        stackText.enabled = stackCount > 1;
    }

    public void ClearSlot()
    {
        _activeEffect = null;

        iconImage.sprite = null;
        iconImage.enabled = false;

        stackText.text = "";
        stackText.enabled = false;

        durationText.text = "";
    }

    public void OnNotify()
    {
        if (_activeEffect != null && _activeEffect.Data.DOTDuration > 0)
        {
            float remaining = Mathf.Max(_activeEffect.Data.DOTDuration - _activeEffect.ElapsedTime, 0f);
            durationText.text = $"{remaining:F1}s";
        }
        else
        {
            durationText.text = "";
        }
    }
}
