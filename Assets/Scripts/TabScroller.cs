using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TabScroller : MonoBehaviour
{
    [SerializeField] float extraPadding = 500f;
    [SerializeField] float scrollStep = 10f;

    float originY;
    float listHeight;
    float screenHeight;
    float additionalHeight;
    float offset;
    float maxOffset;

    RectTransform rectTransform;
    VerticalLayoutGroup verticalLayoutGroup;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        originY = rectTransform.anchoredPosition.y;
        GetHeight();
    }

    void GetHeight()
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            float childHeight = child.GetComponent<RectTransform>().rect.height;
            listHeight += childHeight;
        }

        listHeight += verticalLayoutGroup.spacing * transform.childCount;
        listHeight += extraPadding;

        screenHeight = rectTransform.rect.height;
        
        additionalHeight = listHeight - screenHeight;
        if (additionalHeight > 0f)
        {
            maxOffset = additionalHeight;
        }
        else
        {
            maxOffset = 0f;
        }
    }

    void Update()
    {
        offset = Mathf.Clamp(offset, 0f, maxOffset);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,
                                                     originY + offset);
    }

    void HandleScrollInput(bool scrollingUp)
    {
        float directionMultiplier = 1f;

        if (scrollingUp)
        {
            directionMultiplier = -1f;
        }

        offset += scrollStep * directionMultiplier;
    }

    void OnEnable()
    {
        ShopPlayerInput.OnScrollEvent += HandleScrollInput;
    }

    void OnDisable()
    {
        ShopPlayerInput.OnScrollEvent -= HandleScrollInput;
    }
}
