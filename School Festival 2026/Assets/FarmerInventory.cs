using UnityEngine;
using UnityEngine.Events;

public class FarmerInventory : MonoBehaviour
{
    [SerializeField] private int maxCarry = 3;
    private int carriedRice = 0;

    public int CarriedRice => carriedRice;
    public int MaxCarry => maxCarry;
    public bool IsFull => carriedRice >= maxCarry;

    public UnityEvent<int> OnCarriedChanged; // NEW: fires with new count

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
}