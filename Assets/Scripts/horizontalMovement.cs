using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horizontalMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed;

    [HideInInspector] public OutOfBoundsBox outOfBoundsBox;

    SpriteRenderer spriteRenderer;

    float minX;
    float maxX;

    float direction = 1f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        DetermineScreenBoundaries();

        bool movingRight = Random.value > 0.5f;
        if (movingRight)
        {
            direction = 1f;
            spriteRenderer.flipX = true;
        }
        else
        {
            direction = -1f;
            spriteRenderer.flipX = false;
        }
    }

    void DetermineScreenBoundaries()
    {
        float halfXScale = transform.localScale.x / 2f;

        float halfCameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
        minX = Camera.main.transform.position.x - halfCameraWidth + halfXScale;
        maxX = Camera.main.transform.position.x + halfCameraWidth - halfXScale;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float xPosition = transform.position.x;
        if (xPosition <= minX)
        {
            direction = 1f;
            spriteRenderer.flipX = true;
        }
        if (xPosition >= maxX)
        {
            direction = -1f;
            spriteRenderer.flipX = false;
        }

        
        float newXPosition = xPosition + moveSpeed * direction * Time.deltaTime;

        transform.position = new Vector2 (newXPosition, transform.position.y);
    }
}
