using UnityEngine;

public class BoarSpawner : MonoBehaviour
{
    public GameObject boarPrefab;

    [Header("Spawn Settings")]
    public float spawnDistance = 1f;

    [Header("Lanes")] // NEW
    public float[] laneYPositions = { 3.6f, 2.3f, 1.1f, -1.1f, -2.3f, -3.6f };

    [Header("Normal Spawn Settings")]
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 7f;
    public float normalBoarSpeed = 5f;

    [Header("Raining Settings (faster spawns, faster boars)")]
    public float rainMinSpawnInterval = 1f;
    public float rainMaxSpawnInterval = 3f;
    public float rainBoarSpeed = 8f;

    private float spawnTimer;
    private bool spawningPaused = false;
    private bool isRaining = false;

    private void OnEnable()
    {
        GameStateManager.OnFeverStart += PauseSpawning;
        GameStateManager.OnRainingStart += SwitchToRainSettings;
    }

    private void OnDisable()
    {
        GameStateManager.OnFeverStart -= PauseSpawning;
        GameStateManager.OnRainingStart -= SwitchToRainSettings;
    }

    private void Start()
    {
        spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void Update()
    {
        if (spawningPaused) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnBoar();
            spawnTimer = isRaining
                ? Random.Range(rainMinSpawnInterval, rainMaxSpawnInterval)
                : Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private void PauseSpawning()
    {
        spawningPaused = true;
    }

    private void SwitchToRainSettings()
    {
        spawningPaused = false;
        isRaining = true;
        spawnTimer = Random.Range(rainMinSpawnInterval, rainMaxSpawnInterval);
    }

    private void SpawnBoar()
    {
        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogWarning("Main Camera not found");
            return;
        }

        float width = cam.orthographicSize * cam.aspect;

        // NEW: pick a random lane instead of a random screen edge
        float laneY = laneYPositions[Random.Range(0, laneYPositions.Length)];
        bool fromLeft = Random.value < 0.5f;

        Vector2 spawnPosition = fromLeft
            ? new Vector2(-width - spawnDistance, laneY)
            : new Vector2(width + spawnDistance, laneY);

        Vector2 direction = fromLeft ? Vector2.right : Vector2.left;

        GameObject boarObject = Instantiate(boarPrefab, spawnPosition, Quaternion.identity);
        Boar boar = boarObject.GetComponent<Boar>();

        if (boar != null)
        {
            boar.moveSpeed = isRaining ? rainBoarSpeed : normalBoarSpeed;
            boar.SetDirection(direction);
        }
    }
}