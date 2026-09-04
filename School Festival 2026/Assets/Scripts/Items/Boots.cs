// BootsItem.cs
using UnityEngine;

public class Boots : ItemPickup
{
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float duration = 8f;

    protected override void OnPickedUp(FarmerInventory inventory)
    {
        if (inventory.TryGetComponent(out PlayerCtrl player))
        {
            player.ApplySpeedBoost(speedMultiplier, duration);
        }
    }
}