using UnityEngine;
using GabrielBigardi.SpriteAnimator;

public class FarmerHarvest : MonoBehaviour
{
    [SerializeField] private FarmerInventory inventory;
    [SerializeField] private PlayerCtrl playerCtrl;
    [SerializeField] private HarvestProgressUI progUI;
    [SerializeField] private ScareOffProgressUI crowProgUI;
    [SerializeField] private float harvestDuration = 2f;
    [SerializeField] private float feverHarvestTimeMultiplier = 0.5f;
    [SerializeField] private float scareOffDuration = 2f; 
    [SerializeField] private float crowDetectRadius = 0.6f;
    [SerializeField] private SpriteAnimator spriteAnimator;

    private RiceCrop nearbyCrop;
    private Crow nearbyCrow; 
    private float holdTimer = 0f;
    private bool isHarvesting = false;
    private bool isScaringCrow = false; 
    private float currentHarvestDuration;

    private void Update()
    {
        // NEW — look for a nearby grounded crow every frame when idle, since crows move and aren't trigger-detected
        if (!isHarvesting && !isScaringCrow)
        {
            nearbyCrow = Crow.GetGroundedCrowNear(transform.position, crowDetectRadius);
        }

        if (isHarvesting)
        {
            if (!Input.GetButton("Harvest") || nearbyCrop == null || nearbyCrop.State != RiceCrop.CropState.Ready || playerCtrl.IsStunned)
            {
                CancelHarvest();
                return;
            }

            holdTimer += Time.deltaTime;
            progUI.SetProgress(holdTimer / currentHarvestDuration);

            if (holdTimer >= currentHarvestDuration)
            {
                CompleteHarvest();
            }
        }
        else if (isScaringCrow) // NEW branch
        {
            if (!Input.GetButton("Harvest")  || nearbyCrow == null || nearbyCrow.State != Crow.CrowState.Grounded || playerCtrl.IsStunned)
            {
                CancelScareOff();
                return;
            }

            holdTimer += Time.deltaTime;
            crowProgUI.SetProgress(holdTimer / scareOffDuration);

            if (holdTimer >= scareOffDuration)
            {
                CompleteScareOff();
            }
        }
        else if (Input.GetButtonDown("Harvest"))
        {
            // CHANGED — crow now checked first, since it's the actively-decaying threat
            if (nearbyCrow != null)
            {
                TryStartScareOff();
            }
            else if (nearbyCrop != null && nearbyCrop.State == RiceCrop.CropState.Ready)
            {
                TryStartHarvest();
            }
        }
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
        spriteAnimator.Play("Harvesting");
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
        if (!playerCtrl.IsStunned) // NEW — don't override the stun's own lock/unlock timing
        {
            playerCtrl.SetMovementLocked(false);
        }
        progUI.Hide();
    }

    

    private void TryStartScareOff()
    {
        if (nearbyCrow == null) return;
        if (playerCtrl.IsStunned) return;

        isScaringCrow = true;
        holdTimer = 0f;

        playerCtrl.SetMovementLocked(true);
        crowProgUI.Show();
    }

    private void CompleteScareOff()
    {
        nearbyCrow.ScareOff();
        EndScareOffState();
    }

    private void CancelScareOff()
    {
        EndScareOffState();
    }

    private void EndScareOffState()
    {
        isScaringCrow = false;
        holdTimer = 0f;
        if (!playerCtrl.IsStunned) // NEW — don't override the stun's own lock/unlock timing
        {
            playerCtrl.SetMovementLocked(false);
        }
        crowProgUI.Hide();
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