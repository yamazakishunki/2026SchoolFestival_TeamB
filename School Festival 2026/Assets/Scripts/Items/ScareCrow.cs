
using UnityEngine;

public class ScarecrowItem : ItemPickup
{
    [SerializeField] private float duration = 10f;

    protected override void OnPickedUp(FarmerInventory inventory)
    {
        int randomAreaId = Random.Range(0, 4);
        ItemEffectManager.Instance.BlockArea(randomAreaId, duration);
    }
}