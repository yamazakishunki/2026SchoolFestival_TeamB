
using UnityEngine;
using System.Collections.Generic;

public class Fertilizer : ItemPickup
{
    [SerializeField] private int cropsToRipen = 3;

    protected override void OnPickedUp(FarmerInventory inventory)
    {
        List<RiceCrop> eligible = RiceCrop.GetEligibleForFertilizer();
        int count = Mathf.Min(cropsToRipen, eligible.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, eligible.Count);
            eligible[index].ForceReadyPublic();
            eligible.RemoveAt(index); // don't pick the same crop twice
        }
    }
}