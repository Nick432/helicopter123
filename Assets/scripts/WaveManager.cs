using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<GameObject> waves = new List<GameObject>();

    [SerializeField] Transform spawnMarker;

    GameManager gameManager;

    float distanceTravelled;
    float distanceToNextWave;

    int waveIndex;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        HandleDistanceTravelled();
    }

    void HandleDistanceTravelled()
    {
        float moveSpeed = gameManager.downhillSpeed;
        distanceTravelled += moveSpeed * Time.deltaTime;

        if (distanceTravelled >= distanceToNextWave)
        {
            distanceTravelled = 0f;
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        GameObject wave = waves[waveIndex];

        WaveGizmos wavePrefab = wave.GetComponent<WaveGizmos>();

        distanceToNextWave = wavePrefab.lengthOfWave;

        GameObject[] instances = new GameObject[wave.transform.childCount];

        for (int i = 0; i < instances.Length; i++)
        {
            instances[i] = wave.transform.GetChild(i).gameObject;
        }

        foreach(GameObject instance in instances)
        {
            Vector2 spawnPosition = instance.transform.localPosition + spawnMarker.position;
            Instantiate(instance, spawnPosition, quaternion.identity);

            yield return new WaitForEndOfFrame();
        }

        waveIndex++;

        if (waveIndex > waves.Count - 1)
        {
            waveIndex = 0;
        }
    }
}
