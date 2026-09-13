using UnityEngine;

public class BootsItem : ItemPickup
{
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float duration = 8f;
    [SerializeField] private Sprite statusIcon;

    protected override void OnPickedUp(FarmerInventory inventory)
    {
        if (!inventory.TryGetComponent(out PlayerCtrl player)) return;

        ActiveEffectManager.Instance.ActivateEffect(
            "Boots",
            statusIcon,
            duration,
            onActivate: () => player.SetSpeedMultiplier(speedMultiplier),
            onExpire: () => player.SetSpeedMultiplier(1f)
        );
    }
}