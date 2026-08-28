using UnityEngine;

public class FarmerHarvest : MonoBehaviour
{
    [SerializeField] private FarmerInventory inventory;
    [SerializeField] private KeyCode harvestKey = KeyCode.Space;

    private RiceCrop nearbyCrop;

    private void Update()
    {
        if (Input.GetKeyDown(harvestKey))
        {
            TryHarvestNearby();
        }
    }

    private void TryHarvestNearby()
    {
        if (nearbyCrop == null) return;
        if (nearbyCrop.State != RiceCrop.CropState.Ready) return;
        if (inventory.IsFull) return; // hands full, can't pick more

        Debug.Log("Harvesting crop: " + nearbyCrop.State);
        if (nearbyCrop.TryHarvest())
        {
            inventory.TryAddRice();
        }
    }

    // Requires the farmer to have a Collider2D set to "Is Trigger"
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out RiceCrop crop))
        {
            Debug.Log("Detected: " + crop.name);
            nearbyCrop = crop;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out RiceCrop crop) && crop == nearbyCrop)
        {
            Debug.Log("Exited: " + crop.name);
            nearbyCrop = null;
        }
    }
}