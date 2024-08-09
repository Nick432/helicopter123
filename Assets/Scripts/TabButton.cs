using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    [SerializeField] GameObject tab;
    [SerializeField] Color selectedColour;
    [SerializeField] Color deselectedColour;
    [SerializeField] bool openOnAwake;

    Button button;
    Image image;

    public static event Action<GameObject> OnOpenTab; 

    void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        if (openOnAwake)
        {
            OpenTab();
        }
        else
        {
            CloseTab();
        }
    }

    public void OnButtonClick()
    {
        OnOpenTab.Invoke(this.gameObject);
    }

    void HandleTab(GameObject caller)
    {
        if (caller == this.gameObject)
        {
            OpenTab();
        }
        else
        {
            CloseTab();
        }
        
    }

    void OpenTab()
    {
        button.interactable = false;
        image.color = selectedColour;
        tab.SetActive(true);
    }

    void CloseTab()
    {
        button.interactable = true;
        image.color = deselectedColour;
        tab.SetActive(false);
    }

    void OnEnable()
    {
        OnOpenTab += HandleTab;
    }

    void OnDisable()
    {
        OnOpenTab -= HandleTab;
    }
}
