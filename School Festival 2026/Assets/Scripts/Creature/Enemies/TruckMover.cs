using UnityEngine;
using GabrielBigardi.SpriteAnimator;

public class TruckMover : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteAnimator animator;

    [Header("Front Zone Vertical Movement")]
    [SerializeField] private Transform frontZoneTransform;
    [SerializeField] private BoxCollider2D frontZoneCollider;
    [SerializeField] private float verticalZoneOffset = 0.5f;   // distance from truck center to leading edge
    [SerializeField] private Vector2 verticalZoneSize = new Vector2(0.6f, 0.15f); // wide, thin (matches truck width)

    [Header("Front Zone Horizontal Movement")]
    [SerializeField] private float horizontalZoneOffset = 0.9f; // usually bigger, since side sprite is longer
    [SerializeField] private Vector2 horizontalZoneSize = new Vector2(0.15f, 0.5f); // thin, tall (matches truck height)

    private Vector2 endPos;
    private float speed;
    private GameObject[] commonItemPrefabs;
    private GameObject[] rareItemPrefabs;
    private float rareDropChance;
    private float dropChancePerSecond;


    public void Initialize(Vector2 target, float moveSpeed, GameObject[] commonItems, GameObject[] rareItems,
    float rareChance, float dropChance, Vector2 direction)
    {
        endPos = target;
        speed = moveSpeed;
        commonItemPrefabs = commonItems;
        rareItemPrefabs = rareItems;
        rareDropChance = rareChance;
        dropChancePerSecond = dropChance;

        SetSpriteForDirection(direction);
        PositionAndSizeFrontZone(direction);
    }

    private void SetSpriteForDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.Play("TruckSide");
            spriteRenderer.flipX = direction.x < 0;
        }
        else
        {
            animator.Play(direction.y < 0 ? "TruckFront" : "TruckRear");
            spriteRenderer.flipX = false;
        }
    }

    private void PositionAndSizeFrontZone(Vector2 direction)
    {
        bool movingHorizontally = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);

        if (movingHorizontally)
        {
            float xSign = Mathf.Sign(direction.x);
            frontZoneTransform.localPosition = new Vector2(horizontalZoneOffset * xSign, -0.2f);
            frontZoneCollider.size = horizontalZoneSize;
        }
        else
        {
            float ySign = Mathf.Sign(direction.y);
            frontZoneTransform.localPosition = new Vector2(0f, verticalZoneOffset * ySign);
            frontZoneCollider.size = verticalZoneSize;
        }
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, endPos, speed * Time.deltaTime);

        if (Random.value < dropChancePerSecond * Time.deltaTime)
        {
            DropItem();
        }

        if (Vector2.Distance(transform.position, endPos) < 0.05f)
        {
            Destroy(gameObject);
        }
    }

    private void DropItem()
    {
        GameObject[] pool;

        // Roll for rare first; fall back to common if the roll fails or the rare pool is empty
        if (rareItemPrefabs != null && rareItemPrefabs.Length > 0 && Random.value < rareDropChance)
        {
            pool = rareItemPrefabs;
        }
        else
        {
            pool = commonItemPrefabs;
        }

        if (pool == null || pool.Length == 0) return;

        GameObject prefab = pool[Random.Range(0, pool.Length)];
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}