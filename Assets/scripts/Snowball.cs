using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snowball : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [Header("Player Boundaries")]
    [SerializeField] float leftMargin;
    [SerializeField] float rightMargin;
    [SerializeField] float topMargin;
    [SerializeField] float bottomMargin;
    
    [Header("Scale")]
    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] Transform scaleAnchor;

    Vector2 minScreenBounds;
    Vector2 maxScreenBounds;

    Vector2 moveDirection;

    SnowballSizeManager snowballSizeManager;
    Rigidbody2D myRigidbody2D;

    bool gameOver;

    void Awake()
    {
        snowballSizeManager = FindObjectOfType<SnowballSizeManager>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        FindObjectOfType<GameManager>().HandleOnGameStart();
        SetScreenBoundaries();
    }

    void SetScreenBoundaries()
    {
        // Get camera boundaries in world units.
        Camera mainCamera = Camera.main;
        Vector2 minCameraBounds = mainCamera.ViewportToWorldPoint(new Vector2(0,0));
        Vector2 maxCameraBounds = mainCamera.ViewportToWorldPoint(new Vector2(1,1));

        // Boundaries determined by camera, size of player, and extra margins.
        minScreenBounds = minCameraBounds + new Vector2 (leftMargin, bottomMargin);
        maxScreenBounds = maxCameraBounds - new Vector2 (rightMargin, topMargin);
    }

    // Fixed update is good for working with ridigbody (doesn't need Time.deltaTime)
    void FixedUpdate()
    {
        HandleBoundaries();
        HandleScale();

        if (gameOver) return;

        HandleMovement();
    }

    // This is receiving the input values from the OnMove event that is evoked from the 
    // input action asset.
    void OnMove(InputValue value)
    {
        // We want a direction vector from the raw input.
        Vector2 moveInput = value.Get<Vector2>();
        moveInput.Normalize();
        moveDirection = moveInput;
    }

    void HandleBoundaries()
    {
        // Keep player within boundaries.
        Vector2 position = transform.position;

        //The boundaries need to include half the player's size since the pivot is in the centre.
        float halfScaleX = scaleAnchor.transform.localScale.x / 2f;
        float halfScaleY = scaleAnchor.transform.localScale.y / 2f;

        Vector2 minBounds = minScreenBounds + new Vector2 (halfScaleX, halfScaleY);
        Vector2 maxBounds = maxScreenBounds - new Vector2 (halfScaleX, halfScaleY);

        Vector2 clampedPosition = new Vector2();
        clampedPosition.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        clampedPosition.y = Mathf.Clamp(position.y, minBounds.y, maxBounds.y);

        // If the player has hit the edge of the wall, stop its velocity along the relevant axis.
        // This stops jittery-ness.
        if (clampedPosition.x != position.x)
        {
            myRigidbody2D.velocity = new Vector2(0f, myRigidbody2D.velocity.y);
        }
        if (clampedPosition.y != position.y)
        {
            myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0f);
        }

        // Set clamped position.
        transform.position = clampedPosition;
    }

    void HandleScale()
    {
        // Change scale of snowball according to the size meter
        float sizePercentage = snowballSizeManager.GetSizePercentage();
        float scale = (maxScale - minScale) * sizePercentage + minScale;

        scaleAnchor.localScale = Vector3.one * scale; 
    }

    void HandleMovement()
    {
        // Add force to the rigidbody. This allows the movement to have some variation in speed
        // which can be adjusted by changing the moveSpeed and rigidbody Linear Drag.
        Vector2 moveForce = moveDirection * moveSpeed;
        myRigidbody2D.AddForce(moveForce);
    }

    void OnGameOver()
    {
        gameOver = true;
        scaleAnchor.gameObject.SetActive(false);
    }

    void OnEnable() 
    {
        SnowballSizeManager.OnGameOver += OnGameOver;
    }

    void OnDisable()
    {
        SnowballSizeManager.OnGameOver -= OnGameOver;
    }
}
