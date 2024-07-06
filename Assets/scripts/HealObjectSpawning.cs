using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal.Internal;

public class HealObjectSpawning : MonoBehaviour
{
    [SerializeField] int spawnAmount = 2;
    [SerializeField] float timeBetweenWaves = 7f;
    [SerializeField] float timeBetweenWavesVariance = 1f;
    [SerializeField] float maxTimeBetweenSpawns = 1f;
    [SerializeField] float spawnableRangePadding = 0.5f;

    [SerializeField] List<SpawnChanceElement> objectElements = new List<SpawnChanceElement>();
    [SerializeField] Transform spawnMarker;

    Coroutine spawnWaveCoroutine;

    float minXSpawnPosition;
    float maxXSpawnPosition;

    void Start()
    {
        SortObjectList();
        DetermineSpawnableXPositionRange();
        StartCoroutine(ContinuouslySpawnWaves());
    }

    void SortObjectList()
    {
        objectElements = objectElements.OrderByDescending(x => x.spawnChanceThreshold).ToList();
    }

    void DetermineSpawnableXPositionRange()
    {
        float halfCameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
        minXSpawnPosition = Camera.main.transform.position.x -
                                  halfCameraWidth + spawnableRangePadding;
        maxXSpawnPosition = Camera.main.transform.position.x + 
                                  halfCameraWidth - spawnableRangePadding;
    }

    IEnumerator ContinuouslySpawnWaves()
    {
        // Repeats forever.
        while (true)
        {
            // Get random wait time between waves.
            float randomDelayVariance = Random.Range(-timeBetweenWavesVariance, timeBetweenWavesVariance);
            float delay = timeBetweenWaves + randomDelayVariance;

            yield return new WaitForSeconds(delay);

            if (spawnWaveCoroutine != null)
            {
                StopCoroutine(spawnWaveCoroutine);
                spawnWaveCoroutine = null;
            }
            spawnWaveCoroutine = StartCoroutine(SpawnWave());

        }
        
    }

    IEnumerator SpawnWave()
    {
        // Instantiate objects for spawnAbout times.
        for (int i = 0; i < spawnAmount; i++)
        {
            // Random X position.
            float randomXSpawnPosition = Random.Range(minXSpawnPosition, maxXSpawnPosition);
            Vector2 spawnPosition = new Vector2 (randomXSpawnPosition, spawnMarker.position.y);

            GameObject objectToSpawn = DetermineObjectToSpawn();

            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            // Random delay before next spawn.
            float randomDelay = Random.Range(0f, maxTimeBetweenSpawns);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    GameObject DetermineObjectToSpawn()
    {
        // Get random number.
        float spawnValue = Random.Range(0f, 1f);

        foreach(SpawnChanceElement objectElement in objectElements)
        {
            if (spawnValue > objectElement.spawnChanceThreshold)
            {
                return objectElement.objectPrefab;
            }
        }

        // Default if no suitable object was chosen
        return objectElements[objectElements.Count - 1].objectPrefab;
    }
}
