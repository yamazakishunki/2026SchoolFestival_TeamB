using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowSpawner : MonoBehaviour
{ 
    public GameObject crowPrefab;

    [Header("1st Half Wave Timing")]
    public float minSpawnInterval = 8f;
    public float maxSpawnInterval = 15f;

    [Header("1st Half Crows Per Wave")]
    public int minCrowsPerWave = 1;
    public int maxCrowsPerWave = 3;

    [Header("2nd Half Wave Timing")] 
    public float rainMinSpawnInterval = 5f;
    public float rainMaxSpawnInterval = 9f;

    [Header("2nd Half Crows Per Wave")] 
    public int rainMinCrowsPerWave = 2;
    public int rainMaxCrowsPerWave = 4;

    [Header("Spawn Position")]
    public float spawnHeightAboveScreen = 6f; // how far above the target the crow starts

    private bool spawningPaused = false;
    private bool isRaining = false;

    private void OnEnable() 
    {
        GameStateManager.OnFeverStart += PauseSpawning;
        GameStateManager.OnRainingStart += ResumeSpawning;
        GameStateManager.OnRainingStart += EnableRainSettings;
    }

    private void OnDisable() 
    {
        GameStateManager.OnFeverStart -= PauseSpawning;
        GameStateManager.OnRainingStart -= ResumeSpawning;
        GameStateManager.OnRainingStart -= EnableRainSettings;
    }

    private void PauseSpawning() 
    {
        spawningPaused = true;
    }

    private void ResumeSpawning() 
    {
        spawningPaused = false;
    }

    private void EnableRainSettings() 
    {
        isRaining = true;
    }

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            float minInterval = isRaining ? rainMinSpawnInterval : minSpawnInterval; 
            float maxInterval = isRaining ? rainMaxSpawnInterval : maxSpawnInterval;
            float wait = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(wait);

            if (spawningPaused) continue;

            int minCount = isRaining ? rainMinCrowsPerWave : minCrowsPerWave; 
            int maxCount = isRaining ? rainMaxCrowsPerWave : maxCrowsPerWave;
            int count = Random.Range(minCrowsPerWave, maxCrowsPerWave + 1);
            for (int i = 0; i < count; i++)
            {
                SpawnCrow();
            }
        }
    }

    private void SpawnCrow()
    {
        RiceCrop targetCrop;
        if (ItemEffectManager.Instance != null)
        {
            var blocked = GetCurrentlyBlockedAreas();
            targetCrop = RiceCrop.GetRandomActiveCropExcludingAreas(blocked);
        }
        else
        {
            targetCrop = RiceCrop.GetRandomActiveCrop();
        }

        if (targetCrop == null) return;

        Vector2 targetPos = targetCrop.transform.position;
        Vector2 spawnPos = new Vector2(targetPos.x, targetPos.y + spawnHeightAboveScreen);

        GameObject crowObj = Instantiate(crowPrefab, spawnPos, Quaternion.identity);
        if (crowObj.TryGetComponent(out Crow crow))
        {
            crow.Initialize(targetCrop, spawnHeightAboveScreen);
        }
    }
    private HashSet<int> GetCurrentlyBlockedAreas()
    {
        var set = new HashSet<int>();
        for (int i = 0; i < 4; i++)
        {
            if (ItemEffectManager.Instance.IsAreaBlocked(i)) set.Add(i);
        }
        return set;
    }
}

