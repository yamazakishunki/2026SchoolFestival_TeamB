using UnityEngine;

public class RiceFieldSpawner : MonoBehaviour
{
    [SerializeField] private GameObject riceCropPrefab;
    [SerializeField] private int rows = 3;
    [SerializeField] private int columns = 3;

    [Header("Spacing")]
    [SerializeField] private float horizontalSpacing = 1f; // controls width (X)
    [SerializeField] private float verticalSpacing = 1f;   // controls height (Y)

    [Header("Gizmo Preview")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private Color gizmoColor = Color.green;
    [SerializeField] private float gizmoRadius = 0.3f;

    [Header("Area")]
    [SerializeField] private int areaId;

    private void Start()
    {
        SpawnField();
    }

    private void SpawnField()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 spawnPos = GetGridPosition(row, col);
                GameObject cropObj = Instantiate(riceCropPrefab, spawnPos, Quaternion.identity, transform);
                if (cropObj.TryGetComponent(out RiceCrop crop))
                {
                    crop.SetAreaId(areaId);
                }
            }
        }
    }

    private Vector3 GetGridPosition(int row, int col)
    {
        return transform.position + new Vector3(col * horizontalSpacing, -row * verticalSpacing, 0f);
    }

    // Runs in the Editor even when NOT playing — draws in the Scene view only
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = gizmoColor;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 pos = GetGridPosition(row, col);
                Gizmos.DrawWireSphere(pos, gizmoRadius);
            }
        }
    }
}