using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgility : MonoBehaviour
{
    [SerializeField] float minDrag;
    [SerializeField] float maxDrag;

    float drag;

    Rigidbody2D myRigidbody2D;
    SnowballSizeManager snowballSizeManager;

    void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        snowballSizeManager = GetComponent<SnowballSizeManager>();
    }

    void Update()
    {
        float sizePercentage = snowballSizeManager.GetSizePercentage();
        drag = minDrag + (maxDrag - minDrag) * sizePercentage;
        myRigidbody2D.drag = drag;
    }
}
