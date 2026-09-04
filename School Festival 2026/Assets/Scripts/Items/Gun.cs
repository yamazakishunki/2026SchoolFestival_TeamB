
using UnityEngine;

public class Gun : ItemPickup
{
    protected override void OnPickedUp(FarmerInventory inventory)
    {
        ItemEffectManager.Instance.ClearAllEnemies();
        ScreenFlash.Instance.Flash(Color.white);
    }
}