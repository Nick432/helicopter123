using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveSOElement
{
    public GameObject spawnablePrefab;
    public float timeUntilSpawn = 0f;
    [Range(0f, 1f)] public float positionAcrossScreen;
}
