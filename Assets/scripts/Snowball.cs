using System.Collections;
using System.Collections.Generic;
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
    
    [Header("Size")]
    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] Transform scaleAnchor;

    Vector2 minBounds;
    Vector2 maxBounds;

    Vector2 moveDirection;

    SnowballSizeManager snowballSize;
    Rigidbody2D myRigidbody2D;



    void Awake()
    {
        snowballSize = FindObjectOfType<SnowballSizeManager>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        SetPlayerBoundaries();
    }

    void SetPlayerBoundaries()
    {
        // Get camera boundaries in world units.
        Camera mainCamera = Camera.main;
        Vector2 minCameraBounds = mainCamera.ViewportToWorldPoint(new Vector2(0,0));
        Vector2 maxCameraBounds = mainCamera.ViewportToWorldPoint(new Vector2(1,1));

        // The boundaries need to include half the player's size since the pivot is in the centre.
        float halfScaleX = transform.localScale.x / 2f;
        float halfScaleY = transform.localScale.y / 2f;

        // Boundaries determined by camera, size of player, and extra margins.
        minBounds = minCameraBounds + new Vector2 (halfScaleX + leftMargin, halfScaleY + bottomMargin);
        maxBounds = maxCameraBounds - new Vector2 (halfScaleX + rightMargin, halfScaleY + topMargin);
    }

    // Fixed update is good for working with ridigbody (doesn't need Time.deltaTime)
    void FixedUpdate()
    {
        HandleMovement();
        HandleBoundaries();
        HandleSize();
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

    void HandleMovement()
    {
        // Add force to the rigidbody. This allows the movement to have some variation in speed
        // which can be adjusted by changing the moveSpeed and rigidbody Linear Drag.
        Vector2 moveForce = moveDirection * moveSpeed;
        myRigidbody2D.AddForce(moveForce);
    }

    void HandleBoundaries()
    {
        // Keep player within boundaries.
        Vector2 position = transform.position;
        Vector2 clampedPosition = new Vector2();
        clampedPosition.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        clampedPosition.y = Mathf.Clamp(position.y, minBounds.y, maxBounds.y);

        // Set clamped position.
        transform.position = clampedPosition;

        // Note: this clamping is done after moving, so the player is able to slightly
        // move outside these boundaries.
    }

    void HandleSize()
    {
        // Change scale of snowball according to the size meter
        float sizePercentage = snowballSize.GetSizePercentage();
        float scale = (maxScale - minScale) * sizePercentage + minScale;

        scaleAnchor.localScale = Vector3.one * scale; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Contactable contactDamage = other.GetComponent<Contactable>();
        
        if (contactDamage != null && contactDamage.contactable)
        {
            snowballSize.AddSizePercentage(contactDamage.contactDamage);
            contactDamage.HandleContactBehaviour();
        }
    }
}
