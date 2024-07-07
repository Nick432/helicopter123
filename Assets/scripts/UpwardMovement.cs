using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class UpwardMovement : MonoBehaviour
{
    float moveSpeed;

    Game_Manager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<Game_Manager>();
    }

    void Update()
    {
        moveSpeed = gameManager.globalBaseMoveSpeed;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }
}
