using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgility : MonoBehaviour
{
    [SerializeField] float minDrag = 4f;
    public float gradient = 2f;

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
        float sizeDifferencePercentage = snowballSizeManager.sizeDifferenceFromDefaultPercentage;
        float sizePercentageFromDefault = sizePercentage * sizeDifferencePercentage;

        drag = gradient * sizePercentageFromDefault + minDrag;
        myRigidbody2D.drag = drag;
    }
}
