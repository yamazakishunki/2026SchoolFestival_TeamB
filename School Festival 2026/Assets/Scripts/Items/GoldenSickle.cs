using UnityEngine;

public class GoldenSickleItem : ItemPickup
{
    [SerializeField] private float growthReduction = 1f;
    [SerializeField] private float duration = 10f;
    [SerializeField] private Sprite statusIcon;

    protected override void OnPickedUp(FarmerInventory inventory)
    {
        ActiveEffectManager.Instance.ActivateEffect(
            "GoldenSickle",
            statusIcon,
            duration,
            onActivate: () =>
            {
                // Instant crop-shortening — only fires on first pickup, not on timer-reset re-pickups
                foreach (var crop in RiceCrop.GetEligibleForFertilizer())
                {
                    crop.ReduceRemainingGrowTime(growthReduction);
                }
                ItemEffectManager.SetSickleActive(true, growthReduction); // see ItemEffectManager change below
            },
            onExpire: () =>
            {
                ItemEffectManager.SetSickleActive(false, 0f);
            }
        );
    }
}