using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class FuelSpawning : MonoBehaviour
{
    [SerializeField] int spawnAmount = 2;
    [SerializeField] float timeBetweenWaves = 7f;
    [SerializeField] float timeBetweenWavesVariance = 1f;
    [SerializeField] float maxTimeBetweenSpawns = 1f;

    [SerializeField] GameObject fuelCanPrefab;
    [SerializeField] Transform spawnMarker;

    Coroutine spawnFuelWaveCoroutine;

    float minXSpawnPosition;
    float maxXSpawnPosition;

    void Start()
    {
        DetermineSpawnableXPositionRange();
        StartCoroutine(ContinuouslySpawnFuelWaves());
    }

    void DetermineSpawnableXPositionRange()
    {
        float halfFuelCanPrefabWidth = fuelCanPrefab.transform.localScale.x / 2f;

        float halfCameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
        minXSpawnPosition = Camera.main.transform.position.x -
                                  halfCameraWidth + halfFuelCanPrefabWidth;
        maxXSpawnPosition = Camera.main.transform.position.x + 
                                  halfCameraWidth - halfFuelCanPrefabWidth;
    }

    IEnumerator ContinuouslySpawnFuelWaves()
    {
        // Repeats forever.
        while (true)
        {
            // Get random wait time between waves.
            float randomDelayVariance = Random.Range(-timeBetweenWavesVariance, timeBetweenWavesVariance);
            float delay = timeBetweenWaves + randomDelayVariance;

            yield return new WaitForSeconds(delay);

            if (spawnFuelWaveCoroutine != null)
            {
                StopCoroutine(spawnFuelWaveCoroutine);
                spawnFuelWaveCoroutine = null;
            }
            spawnFuelWaveCoroutine = StartCoroutine(SpawnFuelWave());

        }
        
    }

    IEnumerator SpawnFuelWave()
    {
        // Instantiate fuel cans
        for (int i = 0; i < spawnAmount; i++)
        {
            // Random X position.
            float randomXSpawnPosition = Random.Range(minXSpawnPosition, maxXSpawnPosition);
            Vector2 spawnPosition = new Vector2 (randomXSpawnPosition, spawnMarker.position.y);

            Instantiate(fuelCanPrefab, spawnPosition, Quaternion.identity);

            // Random delay before next spawn.
            float randomDelay = Random.Range(0f, maxTimeBetweenSpawns);
            yield return new WaitForSeconds(randomDelay);
        }
    }
}
