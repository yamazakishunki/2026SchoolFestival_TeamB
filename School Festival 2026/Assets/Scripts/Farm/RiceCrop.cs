using UnityEngine;
using System.Collections.Generic;

public class RiceCrop : MonoBehaviour
{
    public enum CropState { Empty, Growing, Ready }

    [Header("Growth Settings")]
    [SerializeField] private float minGrowTime = 5f;
    [SerializeField] private float maxGrowTime = 7f;
    [SerializeField] private float minStageDisplayTime = 1f;

    [Header("Sprites (3 stages)")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite growingSprite;
    [SerializeField] private Sprite readySprite;

    public CropState State { get; private set; }
    public int AreaId { get; private set; } // NEW ? which plot this crop belongs to (set by RiceFieldSpawner)
    public bool IsHindered { get; set; } = false; // NEW ? true while a Crow is grounded on this tile

    private static readonly List<RiceCrop> activeCrops = new List<RiceCrop>(); // NEW

    private bool isGrowing = false;
    private float growElapsed;
    private float stage1Duration;
    private float totalGrowDuration;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        activeCrops.Add(this); // NEW
        GameStateManager.OnFeverStart += ForceReady;
    }

    private void OnDisable()
    {
        activeCrops.Remove(this); // NEW
        GameStateManager.OnFeverStart -= ForceReady;
    }

    private void Start()
    {
        BeginGrowing();
    }

    public void SetAreaId(int id) // NEW ? called once by RiceFieldSpawner at spawn time
    {
        AreaId = id;
    }

    private void Update()
    {
        if (!isGrowing) return;

        growElapsed += Time.deltaTime;

        if (State == CropState.Empty && growElapsed >= stage1Duration)
        {
            SetState(CropState.Growing);
        }

        if (growElapsed >= totalGrowDuration)
        {
            isGrowing = false;
            SetState(CropState.Ready);
        }
    }

    private void BeginGrowing()
    {
        growElapsed = 0f;
        totalGrowDuration = Random.Range(minGrowTime, maxGrowTime);

        // NEW ? Golden Sickle active when this crop starts a fresh cycle
        if (ItemEffectManager.IsSickleActive)
        {
            totalGrowDuration = Mathf.Max(0.5f, totalGrowDuration - ItemEffectManager.SickleReduction);
        }

        stage1Duration = Mathf.Min(minStageDisplayTime, totalGrowDuration * 0.4f);

        isGrowing = true;
        SetState(CropState.Empty);
    }

    // NEW ? called by Golden Sickle to instantly shorten a crop already mid-growth
    public void ReduceRemainingGrowTime(float amount)
    {
        if (!isGrowing) return;
        totalGrowDuration = Mathf.Max(growElapsed + 0.1f, totalGrowDuration - amount); // keep at least a tiny sliver of time left, avoids an instant same-frame snap feeling glitchy
        stage1Duration = Mathf.Min(stage1Duration, totalGrowDuration);
    }

    public bool TryHarvest()
    {
        if (State != CropState.Ready) return false;
        BeginGrowing();
        return true;
    }

    // NEW ? destroys progress regardless of current state (used by Boar/Crow)
    public void DestroyAndRegrow()
    {
        BeginGrowing();
    }

    // Renamed from ForceReadyImmediately ? now also used by Fertilizer, not just Fever
    private void ForceReady()
    {
        isGrowing = false;
        SetState(CropState.Ready);
    }

    // NEW ? public wrapper so Fertilizer can call it on specific crops (Fever uses the private one via the event)
    public void ForceReadyPublic()
    {
        ForceReady();
    }

    private void SetState(CropState newState)
    {
        State = newState;
        spriteRenderer.sprite = newState switch
        {
            CropState.Empty => emptySprite,
            CropState.Growing => growingSprite,
            CropState.Ready => readySprite,
            _ => spriteRenderer.sprite
        };
    }

    // ---- Static queries used by CrowSpawner / FertilizerItem ----

    public static RiceCrop GetRandomActiveCrop() // NEW
    {
        if (activeCrops.Count == 0) return null;
        return activeCrops[Random.Range(0, activeCrops.Count)];
    }

    public static RiceCrop GetRandomActiveCropExcludingAreas(HashSet<int> blockedAreas) // NEW ? for CrowSpawner
    {
        List<RiceCrop> eligible = new List<RiceCrop>();
        foreach (var crop in activeCrops)
        {
            if (!blockedAreas.Contains(crop.AreaId))
                eligible.Add(crop);
        }
        if (eligible.Count == 0) return null;
        return eligible[Random.Range(0, eligible.Count)];
    }

    public static List<RiceCrop> GetEligibleForFertilizer() // NEW
    {
        List<RiceCrop> eligible = new List<RiceCrop>();
        foreach (var crop in activeCrops)
        {
            if (crop.State != CropState.Ready && !crop.IsHindered)
                eligible.Add(crop);
        }
        return eligible;
    }

    public static List<RiceCrop> GetActiveCropsInArea(int areaId) // NEW ? used by Scarecrow to destroy crows in that area
    {
        List<RiceCrop> result = new List<RiceCrop>();
        foreach (var crop in activeCrops)
        {
            if (crop.AreaId == areaId)
                result.Add(crop);
        }
        return result;
    }
}