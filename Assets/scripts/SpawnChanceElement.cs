using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnChanceElement
{
    public GameObject objectPrefab;
    [Range(0f, 1f)] public float spawnChanceThreshold; 
}
