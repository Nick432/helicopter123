using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    float globalMoveSpeed;

    Material material;
    Vector2 offset;

    void Start()
    {
        globalMoveSpeed = FindObjectOfType<Game_Manager>().globalBaseMoveSpeed;
        material = GetComponent<SpriteRenderer>().material;   
    }

    // Update is called once per frame
    void Update()
    {
        float moveSpeed = globalMoveSpeed / 16f;
        offset = Vector2.down * moveSpeed * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
