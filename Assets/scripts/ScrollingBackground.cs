using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    Game_Manager gameManager;

    Material material;
    Vector2 offset;

    void Start()
    {
        gameManager = FindObjectOfType<Game_Manager>();
        material = GetComponent<SpriteRenderer>().material;   
    }

    void Update()
    {
        float moveSpeed = gameManager.globalBaseMoveSpeed / 16f;
        offset = Vector2.down * moveSpeed * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
