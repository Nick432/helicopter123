using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enhancements : MonoBehaviour
{
    [SerializeField] float[] barValues = new float[7];
    [SerializeField] Image[] barSegments = new Image[7];
    [SerializeField] Sprite lockedSprite;
    [SerializeField] Sprite unlockedSprite;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite highlightedSprite;

    public UnityEvent OnSelectBarSegment;


    GameManager gameManager;
    ShopElement shopElement;

    int selectedIndex = 3;
    int upgradeLevel = 0;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        shopElement = GetComponent<ShopElement>();

        upgradeLevel =  shopElement.GetUpgradeLevel();

        SelectBarSegment(barSegments[selectedIndex]);
        HandleBarSegmentUnlockStates();
    }

    void HandleBarSegmentUnlockStates()
    {
        HashSet<int> unlockedBarSegmentIndexes = new HashSet<int>();

        switch (upgradeLevel)
        {
            case 0:
                unlockedBarSegmentIndexes = new HashSet<int> {3};
                break;
            case 1:
                unlockedBarSegmentIndexes = new HashSet<int> {2, 3, 4};
                break;
            case 2:
                unlockedBarSegmentIndexes = new HashSet<int> {1, 2, 3, 4, 5};
                break;
            case 3:
                unlockedBarSegmentIndexes = new HashSet<int> {0, 1, 2, 3, 4, 5, 6};
                break;
        }

        for (int i = 0; i < barSegments.Length; i++)
        {
            bool unlock = unlockedBarSegmentIndexes.Contains(i);

            Image segmentImage = barSegments[i];
            Button segmentButton = segmentImage.GetComponent<Button>();

            if (unlock)
            {
                segmentImage.sprite = unlockedSprite;
                segmentButton.interactable = true;
                if (i == selectedIndex)
                {
                    segmentImage.sprite = selectedSprite;
                }
            }
            else
            {
                segmentImage.sprite = lockedSprite;
                segmentButton.interactable = false;
            }
        }
    }

    public void UpgradeEnhancement(int newUpgradeLevel)
    {
        upgradeLevel = newUpgradeLevel;

        HandleBarSegmentUnlockStates();
    }

    public void HighlightBarSegment(bool state, GameObject segment)
    {
        Image segmentImage = segment.GetComponent<Image>();
        if (segmentImage == null) return;

        if (state == true && segmentImage.sprite == unlockedSprite)
        {
            segmentImage.sprite = highlightedSprite;
        }
        else if (state == false && segmentImage.sprite == highlightedSprite)
        {
            segmentImage.sprite = unlockedSprite;
        }
    }
    
    public void SelectBarSegment(Image selectedButton)
    {
        for (int i = 0; i < barSegments.Length; i++)
        {
            if (barSegments[i] == selectedButton)
            {
                //Set old selected segment to normal sprite.
                barSegments[selectedIndex].sprite = unlockedSprite;
                //Set new selected segment.
                selectedIndex = i;
                barSegments[selectedIndex].sprite = selectedSprite;

            }
        }

        OnSelectBarSegment.Invoke();
    }

    public void SetMaxSize()
    {
        gameManager.maxSize = barValues[selectedIndex];
    }

    public void SetToughness()
    {
        gameManager.toughness = barValues[selectedIndex];
    }
}
