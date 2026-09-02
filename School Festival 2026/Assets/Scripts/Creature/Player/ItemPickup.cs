using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out FarmerInventory inventory))
        {
            OnPickedUp(inventory);
            Destroy(gameObject);
        }
    }

    // Override this in a subclass per item type once you design their specific effects
    protected virtual void OnPickedUp(FarmerInventory inventory)
    {
        Debug.Log("Picked up: " + gameObject.name);
    }
}