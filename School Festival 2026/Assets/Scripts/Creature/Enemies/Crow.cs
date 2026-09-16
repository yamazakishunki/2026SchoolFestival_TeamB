using UnityEngine;

public class Crow : MonoBehaviour
{
    [SerializeField]private float flyDownSpeed = 4f;
    [SerializeField] private float flyUpSpeed = 4f;
    [SerializeField] private float groundedDuration = 3f;
    [SerializeField] private float arrivalThreshold = 0.05f;
    [SerializeField] private Sprite flySprite;
    [SerializeField] private Sprite landSprite;

    [SerializeField]private SpriteRenderer render;

    public enum CrowState { FlyingDown, Grounded, FlyingUp }
    public CrowState State { get; private set; } = CrowState.FlyingDown; // CHANGED — now public, FarmerHarvest needs to read it

    private RiceCrop targetCrop;
    private Vector2 targetPosition;
    private Vector2 flyAwayPosition;
    private float groundedTimer;
    private bool wasScaredOff = false; // NEW

    private static readonly System.Collections.Generic.List<Crow> activeCrows = new System.Collections.Generic.List<Crow>();

    private void OnEnable()
    {
        activeCrows.Add(this);
    }

    private void OnDisable()
    {
        activeCrows.Remove(this);
    }

    public void Initialize(RiceCrop crop, float spawnHeightAboveScreen)
    {
        targetCrop = crop;
        targetPosition = crop.transform.position;
        flyAwayPosition = new Vector2(targetPosition.x, targetPosition.y + spawnHeightAboveScreen);
    }

    private void Update()
    {
        switch (State)
        {
            case CrowState.FlyingDown:
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, flyDownSpeed * Time.deltaTime);
                render.sprite = flySprite;
                if (Vector2.Distance(transform.position, targetPosition) < arrivalThreshold)
                {
                    Land();
                }
                break;

            case CrowState.Grounded:
                render.sprite = landSprite;
                groundedTimer -= Time.deltaTime;
                if (groundedTimer <= 0f)
                {
                    
                    // NEW — timer ran out naturally, crop gets destroyed NOW instead of at landing
                    if (targetCrop != null)
                    {
                        targetCrop.DestroyAndRegrow();
                    }
                    FlyAway();
                }
                break;

            case CrowState.FlyingUp:
                render.sprite = flySprite;
                transform.position = Vector2.MoveTowards(transform.position, flyAwayPosition, flyUpSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, flyAwayPosition) < arrivalThreshold)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void Land()
    {
        State = CrowState.Grounded;
        groundedTimer = groundedDuration;
        // NEW — crop is NOT destroyed here anymore, only when the grounded timer naturally expires
    }

    // NEW — called by FarmerHarvest when the player successfully finishes the scare-off hold
    public void ScareOff()
    {
        if (State != CrowState.Grounded) return;
        wasScaredOff = true;
        FlyAway(); // crop is untouched — DestroyAndRegrow never gets called for this crow
    }

    private void FlyAway()
    {
        State = CrowState.FlyingUp;   
    }

    public static Crow GetGroundedCrowNear(Vector2 position, float maxDistance) // NEW — used by FarmerHarvest's trigger detection instead
    {
        foreach (var crow in activeCrows)
        {
            if (crow.State == CrowState.Grounded && Vector2.Distance(crow.transform.position, position) <= maxDistance)
            {
                return crow;
            }
        }
        return null;
    }

    public static void DestroyAll()
    {
        var copy = new System.Collections.Generic.List<Crow>(activeCrows);
        foreach (var crow in copy)
        {
            Destroy(crow.gameObject);
        }
    }

    public static void DestroyCrowsInArea(int areaId)
    {
        var copy = new System.Collections.Generic.List<Crow>(activeCrows);
        foreach (var crow in copy)
        {
            if (crow.targetCrop != null && crow.targetCrop.AreaId == areaId)
            {
                Destroy(crow.gameObject);
            }
        }
    }
}