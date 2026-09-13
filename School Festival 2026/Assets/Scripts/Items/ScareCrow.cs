using UnityEngine;

public class ScarecrowItem : ItemPickup
{
    [SerializeField] private float duration = 10f;
    [SerializeField] private Sprite statusIcon;

    protected override void OnPickedUp(FarmerInventory inventory)
    {
        ActiveEffectManager.Instance.ActivateEffect(
            "Scarecrow",
            statusIcon,
            duration,
            onActivate: () =>
            {
                int randomAreaId = Random.Range(0, 4);
                ItemEffectManager.Instance.BlockAreaGlobal(randomAreaId); // see change below
            },
            onExpire: () =>
            {
                ItemEffectManager.Instance.UnblockAreaGlobal();
            }
        );
    }
}