using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel_canister : MonoBehaviour
{
    [SerializeField] float baseMoveSpeed = 10f;
    [SerializeField] float moveSpeedVariance = 1f;
    [Tooltip("Amount of fuel to refill in Seconds")] public float refillAmount = 10f;

    float moveSpeed;

    void Start()
    {
        moveSpeed = Random.Range(baseMoveSpeed - moveSpeedVariance, 
                                       baseMoveSpeed + moveSpeedVariance);
    }

    void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }

}
