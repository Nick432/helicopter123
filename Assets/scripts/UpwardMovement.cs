using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class UpwardMovement : MonoBehaviour
{
    [SerializeField] bool overwriteGlobalMoveSpeed;
    [SerializeField] float customMoveSpeed;
    [SerializeField] float moveSpeedVariance;

    float initialMoveSpeed;
    float moveSpeed;

    Game_Manager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<Game_Manager>();
    }

    void Start()
    {
        if (overwriteGlobalMoveSpeed)
        {
            initialMoveSpeed = customMoveSpeed;
        }
        else
        {
            initialMoveSpeed = gameManager.globalBaseMoveSpeed;
        }

        moveSpeed = Random.Range(initialMoveSpeed - moveSpeedVariance, 
                                       initialMoveSpeed + moveSpeedVariance);
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }

}
