using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] GameObject expandedMenu;
    [SerializeField] RectTransform layoutGroupRect;

    float collapsedHeight;
    float expandedHeight;

    RectTransform rectTransform;

    bool menuIsExpanded;

    public static event Action OnExpandedMenuClicked;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        collapsedHeight = rectTransform.rect.height;
        expandedHeight = expandedMenu.GetComponent<RectTransform>().rect.height;
    }

    public void ExpandedMenuClicked()
    {
        if (menuIsExpanded)
        {
            OnExpandedMenuClicked -= ExpandedMenuClicked;
            ExpandItemMenu(false);
        }
        else
        {
            OnExpandedMenuClicked?.Invoke();
            OnExpandedMenuClicked += ExpandedMenuClicked;
            ExpandItemMenu(true);
        }
    }
    
    void ExpandItemMenu(bool state)
    {
        menuIsExpanded = state;

        if (state == true)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, expandedHeight);

            expandedMenu.SetActive(true);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, collapsedHeight);

            expandedMenu.SetActive(false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroupRect);
    }

    void OnEnable()
    {
        if (menuIsExpanded)
        {
            OnExpandedMenuClicked += ExpandedMenuClicked;
        }
    }

    void OnDisable()
    {
        OnExpandedMenuClicked -= ExpandedMenuClicked;
    }
}
