using System.Collections;
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
    private static readonly List<RiceCrop> activeCrops = new List<RiceCrop>();
    private Coroutine growRoutine;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

     

    private void OnEnable()
    {
        activeCrops.Add(this); 
        GameStateManager.OnFeverStart += ForceReadyImmediately; // existing, keep this
    }

    private void OnDisable()
    {
        activeCrops.Remove(this); 
        GameStateManager.OnFeverStart -= ForceReadyImmediately; // existing, keep this
    }

    // NEW ? lets CrowSpawner pick a random tile to target without an expensive FindObjectsOfType search
    public static RiceCrop GetRandomActiveCrop()
    {
        if (activeCrops.Count == 0) return null;
        return activeCrops[Random.Range(0, activeCrops.Count)];
    }

    private void Start()
    {
        BeginGrowing();
    }

    private void BeginGrowing()
    {
        if (growRoutine != null) StopCoroutine(growRoutine);
        growRoutine = StartCoroutine(GrowSequence());
    }

    private IEnumerator GrowSequence()
    {
        // NEW: if Fever is already active when this crop starts regrowing, skip straight to Ready
        if (GameStateManager.Instance != null && GameStateManager.Instance.CurrentState == GameStateManager.GameState.Fever)
        {
            SetState(CropState.Ready);
            yield break;
        }

        float totalGrowTime = Random.Range(minGrowTime, maxGrowTime);

        SetState(CropState.Empty);
        float stage1Time = Mathf.Min(minStageDisplayTime, totalGrowTime * 0.4f);
        yield return new WaitForSeconds(stage1Time);

        SetState(CropState.Growing);
        float stage2Time = Mathf.Max(minStageDisplayTime, totalGrowTime - stage1Time);
        yield return new WaitForSeconds(stage2Time);

        SetState(CropState.Ready);
    }

    // NEW: called on every crop the instant Fever starts, regardless of what stage it's currently in
    private void ForceReadyImmediately()
    {
        if (growRoutine != null) StopCoroutine(growRoutine);
        SetState(CropState.Ready);
    }

    public bool TryHarvest()
    {
        if (State != CropState.Ready) return false;

        BeginGrowing();
        return true;
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

    public void DestroyAndRegrow()
    {
        BeginGrowing(); // stops any in-progress coroutine and restarts from Empty
    }
}