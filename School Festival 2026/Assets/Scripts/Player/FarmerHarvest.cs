using UnityEngine;

public class FarmerHarvest : MonoBehaviour
{
    [SerializeField] private FarmerInventory inventory;
    [SerializeField] private PlayerCtrl playerCtrl;
    [SerializeField] private KeyCode harvestKey = KeyCode.Space;
    [SerializeField] private float harvestDuration = 2f;
    [SerializeField] private float feverHarvestMultiplier = 0.5f; // NEW — twice as fast = half the duration

    private RiceCrop nearbyCrop;
    private float holdTimer = 0f;
    private bool isHarvesting = false;
    private float currentHarvestDuration; // NEW — locked in when harvest starts, so speeding up/slowing down mid-hold doesn't retroactively change progress

    private void Update()
    {
        if (isHarvesting)
        {
            if (!Input.GetKey(harvestKey) || nearbyCrop == null || nearbyCrop.State != RiceCrop.CropState.Ready)
            {
                CancelHarvest();
                return;
            }

            holdTimer += Time.deltaTime;

            if (holdTimer >= currentHarvestDuration) // NEW — uses the locked-in duration
            {
                CompleteHarvest();
            }
        }
        else if (Input.GetKeyDown(harvestKey))
        {
            TryStartHarvest();
        }
    }

    private void TryStartHarvest()
    {
        if (nearbyCrop == null) return;
        if (nearbyCrop.State != RiceCrop.CropState.Ready) return;
        if (inventory.IsFull) return;

        isHarvesting = true;
        holdTimer = 0f;

        // NEW: decide this harvest's duration based on current state, right when it starts
        bool isFever = GameStateManager.Instance != null
            && GameStateManager.Instance.CurrentState == GameStateManager.GameState.Fever;
        currentHarvestDuration = isFever ? harvestDuration * feverHarvestMultiplier : harvestDuration;

        playerCtrl.SetMovementLocked(true);
    }

    private void CompleteHarvest()
    {
        if (nearbyCrop.TryHarvest())
        {
            inventory.TryAddRice();
        }
        EndHarvestState();
    }

    private void CancelHarvest()
    {
        EndHarvestState();
    }

    private void EndHarvestState()
    {
        isHarvesting = false;
        holdTimer = 0f;
        playerCtrl.SetMovementLocked(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out RiceCrop crop))
        {
            nearbyCrop = crop;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out RiceCrop crop) && crop == nearbyCrop)
        {
            if (isHarvesting) CancelHarvest();
            nearbyCrop = null;
        }
    }
}