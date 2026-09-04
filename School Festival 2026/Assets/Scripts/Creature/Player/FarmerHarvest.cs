using UnityEngine;
using UnityEngine.UI;

public class FarmerHarvest : MonoBehaviour
{
    [SerializeField] private FarmerInventory inventory;
    [SerializeField] private PlayerCtrl playerCtrl;
    [SerializeField] private KeyCode harvestKey = KeyCode.Space;
    [SerializeField] private float harvestDuration = 2f;
    [SerializeField] private float feverHarvestTimeMultiplier = 0.5f;
    [SerializeField] private HarvestProgressUI progUI;

    private RiceCrop nearbyCrop;
    private float holdTimer = 0f;
    private bool isHarvesting = false;
    private float currentHarvestDuration;

    private void Update()
    {
        if (isHarvesting)
        {
            if (!Input.GetButton("Harvest")) 
            { 
                Debug.Log("Cancelled: button released"); CancelHarvest(); return;
            }
            if (nearbyCrop == null) 
            { 
                Debug.Log("Cancelled: nearbyCrop is null"); CancelHarvest(); return; 
            }
            if (nearbyCrop.State != RiceCrop.CropState.Ready) 
            { 
                Debug.Log("Cancelled: crop state is " + nearbyCrop.State); CancelHarvest(); return; 
            }

            holdTimer += Time.deltaTime;
            progUI.SetProgress(holdTimer / currentHarvestDuration);

            if (holdTimer >= currentHarvestDuration)
            {
                CompleteHarvest();
            }
        }
        else if (Input.GetKeyDown(harvestKey) || Input.GetButton("Harvest"))
        {
            TryStartHarvest();
        }
        Debug.Log(Input.GetButton("Harvest"));
        
    }

    private void TryStartHarvest()
    {
        if (nearbyCrop == null) return;
        if (nearbyCrop.State != RiceCrop.CropState.Ready) return;
        if (inventory.IsFull) return;
        if (playerCtrl.IsStunned) return; 

        isHarvesting = true;
        holdTimer = 0f;

        bool isFever = GameStateManager.Instance != null
            && GameStateManager.Instance.CurrentState == GameStateManager.GameState.Fever;
        currentHarvestDuration = isFever ? harvestDuration * feverHarvestTimeMultiplier : harvestDuration;

        playerCtrl.SetMovementLocked(true);
        progUI.Show();
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
        progUI.Hide();
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