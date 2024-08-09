using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadingCanvas : MonoBehaviour
{
    [Header("Tab Objects")]
    [SerializeField] GameObject enhancementsTab;
    [SerializeField] GameObject itemsTab;
    [SerializeField] GameObject abilitiesTab;
    [Header("Tab Buttons")]
    [SerializeField] Button enhancementsButton;
    [SerializeField] Button itemsButton;
    [SerializeField] Button abilitiesButton;
    [SerializeField] Color selectedColour;
    [SerializeField] Color deselectedColour;

    GameObject[] tabObjects;
    Button[] tabButtons;

    void Start()
    {
        tabObjects = new GameObject[] {enhancementsTab, itemsTab, abilitiesTab};
        tabButtons = new Button[] {enhancementsButton, itemsButton, abilitiesButton};
    }

    public void OpenTab(GameObject openTab)
    {
        for (int i = 0; i < tabObjects.Length; i++)
        {
            if (tabObjects[i] == openTab)
            {
                tabObjects[i].SetActive(true);
                ColorBlock buttonColours = tabButtons[i].colors;
                buttonColours.normalColor = selectedColour;
                tabButtons[i].colors = buttonColours;
            }
            else
            {
                tabObjects[i].SetActive(false);
                ColorBlock buttonColours = tabButtons[i].colors;
                buttonColours.normalColor = deselectedColour;
                tabButtons[i].colors = buttonColours;
            }
        }
    }
}
