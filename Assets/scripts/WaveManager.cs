using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<WaveSO> waves = new List<WaveSO>();

    [SerializeField] Transform spawnMarker;

    Coroutine spawnWaveCoroutine;
    Game_Manager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<Game_Manager>();
        StartCoroutine(SpawnWavesContinuously());   
    }

    IEnumerator SpawnWavesContinuously()
    {
        // Spawn waves indefinitely.
        while(true)
        {
            foreach (WaveSO wave in waves)
            {
                float delay = GetConsistentTime(wave.timeUntilWaveStarts);
                yield return new WaitForSeconds(wave.timeUntilWaveStarts);

                spawnWaveCoroutine = StartCoroutine(SpawnWave(wave));

                yield return new WaitUntil(() => spawnWaveCoroutine == null);

                gameManager.IncreaseTopDownhillSpeed();
            }
        }
        
    }

    IEnumerator SpawnWave(WaveSO wave)
    {
        // Spawn each object in wave.
        foreach (WaveSOElement waveElement in wave.waveElements)
        {
            float delay = GetConsistentTime(waveElement.timeUntilSpawn);
            yield return new WaitForSeconds(delay);

            SpawnWaveObject(waveElement);
        }

        spawnWaveCoroutine = null;
    }

    float GetConsistentTime(float delayTime)
    {
        float initialGlobalBaseMoveSpeed = gameManager.initialDownhillSpeed;
        float currentGlobalBaseMoveSpeed = gameManager.downhillSpeed;

        float speedIncreaseFactor = currentGlobalBaseMoveSpeed / initialGlobalBaseMoveSpeed;

        if (speedIncreaseFactor == 0f)
        {
            speedIncreaseFactor = 0.01f;
        }
        return delayTime / speedIncreaseFactor;
    }

    void SpawnWaveObject(WaveSOElement waveElement)
    {
        float xSpawnPosition = DetermineXSpawnPosition(waveElement);

        Vector2 position = new Vector2(xSpawnPosition, spawnMarker.position.y);

        Instantiate(waveElement.spawnablePrefab, position, Quaternion.identity);
    }

    // Returns x spawn position along the bottom of the screen for the wave element object.
    float DetermineXSpawnPosition(WaveSOElement waveElement)
    {
        float objectPrefabWidth = waveElement.spawnablePrefab.transform.localScale.x;

        float cameraWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
        float spawnableWidth = cameraWidth - objectPrefabWidth;

        float leftmostSpawnablePosition = Camera.main.transform.position.x - spawnableWidth / 2f;
        float percentageAlongSpawnableWidth = waveElement.positionAcrossScreen;
        float xSpawnPosition = spawnableWidth * percentageAlongSpawnableWidth + leftmostSpawnablePosition;

        return xSpawnPosition;
    }

}
