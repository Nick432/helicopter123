using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpwardMovement : MonoBehaviour
{
    float moveSpeed;

    DownhillSpeedManager downhillSpeedManager;

    void Awake()
    {
        downhillSpeedManager = FindObjectOfType<DownhillSpeedManager>();
    }

    void Update()
    {
        moveSpeed = downhillSpeedManager.downhillSpeed;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }
}
