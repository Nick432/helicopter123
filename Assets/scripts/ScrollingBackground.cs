using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    DownhillSpeedManager downhillSpeedManager;

    Material material;
    Vector2 offset;

    void Start()
    {
        downhillSpeedManager = FindObjectOfType<DownhillSpeedManager>();
        material = GetComponent<SpriteRenderer>().material;   
    }

    void Update()
    {
        float moveSpeed = downhillSpeedManager.downhillSpeed / 16f;
        offset = Vector2.down * moveSpeed * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
