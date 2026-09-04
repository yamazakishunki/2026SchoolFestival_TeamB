using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemEffectManager : MonoBehaviour
{
    public static ItemEffectManager Instance { get; private set; }

    // ---- Golden Sickle ----
    public static bool IsSickleActive { get; private set; }
    public static float SickleReduction { get; private set; }

    // ---- Scarecrow ----
    private readonly Dictionary<int, Coroutine> blockedAreaRoutines = new Dictionary<int, Coroutine>();
    private readonly HashSet<int> blockedAreas = new HashSet<int>();

    private void Awake()
    {
        Instance = this;
    }

    public void ActivateGoldenSickle(float reduction, float duration)
    {
        // Instantly shave time off every crop currently growing
        foreach (var crop in RiceCrop.GetEligibleForFertilizer()) // reuses "not ready" query ? sickle shouldn't touch already-ready crops anyway
        {
            crop.ReduceRemainingGrowTime(reduction);
        }

        SickleReduction = reduction;
        IsSickleActive = true;
        StartCoroutine(SickleTimer(duration));
    }

    private IEnumerator SickleTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        IsSickleActive = false;
    }

    public bool IsAreaBlocked(int areaId)
    {
        return blockedAreas.Contains(areaId);
    }

    public void BlockArea(int areaId, float duration)
    {
        // Destroy any crow currently targeting a crop in this area
        Crow.DestroyCrowsInArea(areaId);

        blockedAreas.Add(areaId);

        // If already blocked, restart the timer instead of stacking
        if (blockedAreaRoutines.TryGetValue(areaId, out Coroutine existing))
        {
            StopCoroutine(existing);
        }
        blockedAreaRoutines[areaId] = StartCoroutine(AreaBlockTimer(areaId, duration));
    }

    private IEnumerator AreaBlockTimer(int areaId, float duration)
    {
        yield return new WaitForSeconds(duration);
        blockedAreas.Remove(areaId);
        blockedAreaRoutines.Remove(areaId);
    }

    // ---- Gun ----
    public void ClearAllEnemies()
    {
        Boar.DestroyAll();
        Crow.DestroyAll();
    }
}