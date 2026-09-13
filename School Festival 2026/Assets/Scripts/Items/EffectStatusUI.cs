using UnityEngine;
using UnityEngine.UI;

public class EffectStatusUI : MonoBehaviour
{
    [System.Serializable]
    public class EffectSlot
    {
        public GameObject container; // parent holding icon + fill bar for this slot
        public Image icon;
        public Image fillBar; // Image Type = Filled, same pattern as HarvestProgressUI
    }

    [SerializeField] private EffectSlot[] slots = new EffectSlot[3];

    private void OnEnable()
    {
        ActiveEffectManager.Instance.OnEffectsChanged += RefreshDisplay;
    }

    private void OnDisable()
    {
        ActiveEffectManager.Instance.OnEffectsChanged -= RefreshDisplay;
    }

    private void Update()
    {
        // Fill bars need continuous updating for smooth countdown, not just on change events
        var effects = ActiveEffectManager.Instance.GetActiveEffects();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < effects.Count)
            {
                slots[i].container.SetActive(true);
                slots[i].icon.sprite = effects[i].Icon;
                slots[i].fillBar.fillAmount = effects[i].Remaining / effects[i].Duration;
            }
            else
            {
                slots[i].container.SetActive(false);
            }
        }
    }

    private void RefreshDisplay()
    {
        // Update() already handles the visuals every frame; this just exists in case you want
        // an immediate visual "pop" animation on change later (e.g. icon scale-in on activate)
    }
}