using UnityEngine;

public class ItemEffectManager : MonoBehaviour
{
    public static ItemEffectManager Instance { get; private set; }

    // ---- Golden Sickle ----
    public static bool IsSickleActive { get; private set; }
    public static float SickleReduction { get; private set; }

    public static void SetSickleActive(bool active, float reduction)
    {
        IsSickleActive = active;
        SickleReduction = reduction;
    }

    // ---- Scarecrow  ----
    private int blockedAreaId = -1; 

    public static event System.Action<int> OnAreaBlocked;   
    public static event System.Action OnAreaUnblocked;

    private void Awake()
    {
        Instance = this;
    }

    public void BlockAreaGlobal(int areaId)
    {
        Crow.DestroyCrowsInArea(areaId);
        blockedAreaId = areaId;
        OnAreaBlocked?.Invoke(areaId);
    }

    public void UnblockAreaGlobal()
    {
        blockedAreaId = -1;
        OnAreaUnblocked?.Invoke();
    }

    public bool IsAreaBlocked(int areaId)
    {
        return areaId == blockedAreaId;
    }

    // ---- Gun ----
    public void ClearAllEnemies()
    {
        Boar.DestroyAll();
        Crow.DestroyAll();
    }
}