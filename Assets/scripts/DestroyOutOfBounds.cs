using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    // This script can be attached to anything that will be destroyed when moving offscreen.
    // E.g. fuel cans, obstacles, projectiles.

    float minX;
    float maxX;
    float minY;
    float maxY;

    bool isOutOfBounds;

    void Start()
    {
        GetBoundaryBoxPoints();
    }
    
    void GetBoundaryBoxPoints()
    {
        // Find the boundary box in scene.
        OutOfBoundsBox outOfBoundsBox = FindObjectOfType<OutOfBoundsBox>();
        if (outOfBoundsBox == null) return;

        Vector2 bottomLeftBoxPoint = outOfBoundsBox.bottomLeftBoxPoint;
        Vector2 topRightBoxPoint = outOfBoundsBox.topRightBoxPoint;

        minX = bottomLeftBoxPoint.x;
        maxX = topRightBoxPoint.x;
        minY = bottomLeftBoxPoint.y;
        maxY = topRightBoxPoint.y;
    }

    void Update()
    {
        // Check if position is within bounds or not.
        float xPosition = transform.position.x;

        if (xPosition < minX || xPosition > maxX)
        {
            isOutOfBounds = true;
        }
        
        float yPosition = transform.position.y;

        if (yPosition < minY || yPosition > maxY)
        {
            isOutOfBounds = true;
        }

        // Destroy when out of bounds.
        if (isOutOfBounds)
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
