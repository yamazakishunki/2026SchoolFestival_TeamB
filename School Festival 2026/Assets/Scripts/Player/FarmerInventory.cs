using UnityEngine;
using UnityEngine.Events;

public class FarmerInventory : MonoBehaviour
{
    [SerializeField] private int maxCarry = 3;
    private int carriedRice = 0;

    public int CarriedRice => carriedRice;
    public int MaxCarry => maxCarry;

    // NEW: infinite carry while Fever is active
    public bool IsFull =>
        GameStateManager.Instance != null && GameStateManager.Instance.CurrentState == GameStateManager.GameState.Fever
            ? false
            : carriedRice >= maxCarry;

    public UnityEvent<int> OnCarriedChanged;

    private void OnEnable()
    {
        GameStateManager.OnRainingStart += ClampInventoryForRaining; // NEW
    }

    private void OnDisable()
    {
        GameStateManager.OnRainingStart -= ClampInventoryForRaining; // NEW
    }

    public bool TryAddRice()
    {
        if (IsFull) return false;
        carriedRice++;
        OnCarriedChanged?.Invoke(carriedRice);
        return true;
    }

    public int RemoveAllRice()
    {
        int amount = carriedRice;
        carriedRice = 0;
        OnCarriedChanged?.Invoke(carriedRice);
        return amount;
    }

    // NEW: rule from Fever → Raining transition
    private void ClampInventoryForRaining()
    {
        if (carriedRice >= 4)
        {
            carriedRice = 3;
            OnCarriedChanged?.Invoke(carriedRice);
        }
        // 3 or fewer: left untouched
    }
}