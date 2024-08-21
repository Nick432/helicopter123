using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BestDistanceText : MonoBehaviour
{
    [SerializeField] RectTransform target;
    [SerializeField] float leftPadding;
    [SerializeField] float rightPadding;
    [SerializeField] float xMaxAnchor = 0.7f;

    float minX;
    float maxX;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = transform.GetComponent<RectTransform>();
    }

    void Start()
    {
        Vector2 minBounds = Camera.main.ViewportToScreenPoint(Vector2.zero);
        Vector2 maxBounds = Camera.main.ViewportToScreenPoint(Vector2.one);
        
        minX = minBounds.x + leftPadding;
        maxX = maxBounds.x * xMaxAnchor - rightPadding;
    }

    void Update()
    {
        float newXPosition = target.position.x;
        newXPosition = Mathf.Clamp(newXPosition, minX, maxX);

        rectTransform.position = new Vector2(newXPosition, rectTransform.position.y);
    }

}
