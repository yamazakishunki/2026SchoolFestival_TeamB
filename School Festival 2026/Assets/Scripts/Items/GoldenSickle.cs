// GoldenSickleItem.cs
using UnityEngine;

public class GoldenSickle : ItemPickup
{
    [SerializeField] private float growthReduction = 1f;
    [SerializeField] private float duration = 10f;

    protected override void OnPickedUp(FarmerInventory inventory)
    {
        ItemEffectManager.Instance.ActivateGoldenSickle(growthReduction, duration);
    }
}