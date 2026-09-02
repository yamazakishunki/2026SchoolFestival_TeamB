using UnityEngine;
using System.Collections;

public class TruckManager : MonoBehaviour
{
    public enum SpawnEdge { Left, Right, Top, Bottom }

    [Header("Spawn Settings")]
    [SerializeField] private GameObject truckPrefab;
    [SerializeField] private float spawnInterval = 17f;
    [SerializeField] private float truckSpeed = 4f;

    [Header("Warning Sign")]
    [SerializeField] private GameObject warningSignPrefab; 
    [SerializeField] private float warningDuration = 1.5f; 
    [SerializeField] private float warningInset = 0.5f;    

    [Header("Screen Bounds")]
    [SerializeField] private float screenHalfWidth = 8f;
    [SerializeField] private float screenHalfHeight = 4.5f;
    [SerializeField] private float spawnBuffer = 1f;

    [Header("Item Drops")]
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private float dropChancePerSecond = 0.3f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            StartCoroutine(SpawnTruckWithWarning()); 
        }
    }

    private IEnumerator SpawnTruckWithWarning() 
    {
        SpawnEdge edge = (SpawnEdge)Random.Range(0, 4);
        GetSpawnAndTargetPositions(edge, out Vector2 startPos, out Vector2 endPos);

        Vector2 warningPos = GetWarningPosition(edge, startPos);
        GameObject warningSign = Instantiate(warningSignPrefab, warningPos, Quaternion.identity);

        yield return new WaitForSeconds(warningDuration);

        Destroy(warningSign);
        SpawnTruckAt(startPos, endPos);
    }

    private Vector2 GetWarningPosition(SpawnEdge edge, Vector2 truckSpawnPos) // NEW
    {
        // Places the sign just inside the visible screen edge, aligned with the truck's travel line
        switch (edge)
        {
            case SpawnEdge.Left:
                return new Vector2(-screenHalfWidth + warningInset, truckSpawnPos.y);
            case SpawnEdge.Right:
                return new Vector2(screenHalfWidth - warningInset, truckSpawnPos.y);
            case SpawnEdge.Top:
                return new Vector2(truckSpawnPos.x, screenHalfHeight - warningInset);
            default: // Bottom
                return new Vector2(truckSpawnPos.x, -screenHalfHeight + warningInset);
        }
    }

    private void SpawnTruckAt(Vector2 startPos, Vector2 endPos) // renamed from SpawnTruck, now takes positions directly
    {
        GameObject truck = Instantiate(truckPrefab, startPos, Quaternion.identity);
        Vector2 direction = (endPos - startPos).normalized;

        if (truck.TryGetComponent(out TruckMover mover))
        {
            mover.Initialize(endPos, truckSpeed, itemPrefabs, dropChancePerSecond, direction);
        }
    }

    private void GetSpawnAndTargetPositions(SpawnEdge edge, out Vector2 start, out Vector2 end)
    {
        switch (edge)
        {
            case SpawnEdge.Left:
                start = new Vector2(-screenHalfWidth - spawnBuffer, 0);
                end = new Vector2(screenHalfWidth + spawnBuffer, start.y);
                break;
            case SpawnEdge.Right:
                start = new Vector2(screenHalfWidth + spawnBuffer, 0);
                end = new Vector2(-screenHalfWidth - spawnBuffer, start.y);
                break;
            case SpawnEdge.Top:
                start = new Vector2(0, screenHalfHeight + spawnBuffer);
                end = new Vector2(start.x, -screenHalfHeight - spawnBuffer);
                break;
            default: // Bottom
                start = new Vector2(0, -screenHalfHeight - spawnBuffer);
                end = new Vector2(start.x, screenHalfHeight + spawnBuffer);
                break;
        }
    }
}