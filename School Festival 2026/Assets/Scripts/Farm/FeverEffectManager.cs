using UnityEngine;


public class FeverEffectManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameStateManager.OnFeverStart += HandleFeverStart;
    }

    private void OnDisable()
    {
        GameStateManager.OnFeverStart -= HandleFeverStart;
    }

    private void HandleFeverStart()
    {
        ItemPickup.DestroyAllGroundItems(); // Rule 1 ? clear dropped items
        Boar.DestroyAll();                  // Rule 2 ? clear existing enemies
        Crow.DestroyAll();
        // Rule 3 (instant crop growth) already handled by RiceCrop's own OnFeverStart subscription
    }
}
