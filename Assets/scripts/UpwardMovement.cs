using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpwardMovement : MonoBehaviour
{
    float moveSpeed;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        moveSpeed = gameManager.downhillSpeed;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }
}
