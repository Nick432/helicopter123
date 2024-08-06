using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    GameManager gameManager;

    Material material;
    Vector2 offset;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        material = GetComponent<SpriteRenderer>().material;   
    }

    void Update()
    {
        float moveSpeed = gameManager.downhillSpeed / 16f;
        offset = Vector2.down * moveSpeed * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
