using System.Collections;
using UnityEngine;

public class RiceCrop : MonoBehaviour
{
    public enum CropState { Empty, Sprout, Growing, Ready }

    [Header("Growth Settings")]
    [SerializeField] private float minGrowTime = 5f;
    [SerializeField] private float maxGrowTime = 7f;
    [SerializeField] private float minStageDisplayTime = 1f; 

    [Header("Sprites (3 stages)")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite sproutSprite;
    [SerializeField] private Sprite growingSprite;
    [SerializeField] private Sprite readySprite;

    public CropState State { get; private set; }

    private Coroutine growRoutine;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
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
        // Total random grow duration for this cycle
        float totalGrowTime = Random.Range(minGrowTime, maxGrowTime);

        // Stage 1: "empty/sprout" sprite, shown for at least minStageDisplayTime
        SetState(CropState.Empty);
        float stage0Time = Mathf.Min(minStageDisplayTime, totalGrowTime * 0.4f);
        yield return new WaitForSeconds(stage0Time);

        SetState(CropState.Sprout);
        float stage1SproutTime = Mathf.Max(minStageDisplayTime, totalGrowTime * 0.4f);
        yield return new WaitForSeconds(stage1SproutTime);

        // Stage 2: "growing" sprite, shown for the remaining time (at least minStageDisplayTime)
        SetState(CropState.Growing);
        float stage2Time = Mathf.Max(minStageDisplayTime, totalGrowTime - stage0Time);
        yield return new WaitForSeconds(stage2Time);

        // Stage 3: ready to harvest
        SetState(CropState.Ready);
    }

    public bool TryHarvest()
    {
        if (State != CropState.Ready) return false;

        BeginGrowing(); // restart the whole sequence from stage 0
        return true;
    }

    private void SetState(CropState newState)
    {
        State = newState;
        spriteRenderer.sprite = newState switch
        {
            CropState.Empty => emptySprite,
            CropState.Sprout => sproutSprite,
            CropState.Growing => growingSprite,
            CropState.Ready => readySprite,
            _ => spriteRenderer.sprite
        };
    }
}