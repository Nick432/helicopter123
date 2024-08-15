using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class LevelStar : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] Image textboxImage;
    [SerializeField] Image textboxMarker; 
    [SerializeField] GameObject textbox;
    [SerializeField] TextMeshProUGUI distanceText;

    [Header("Sprites")]
    [SerializeField] Sprite lockedStarSprite;
    [SerializeField] Sprite unlockedStarSprite;
    [SerializeField] Sprite textboxImageLocked;
    [SerializeField] Sprite textboxImageUnlocked;
    [SerializeField] Sprite textboxMarkerLocked;
    [SerializeField] Sprite textboxMarkerUnlocked;

    bool unlocked;
    int unlockDistance;

    Image starImage;

    void Awake()
    {
        starImage = GetComponent<Image>();
    }

    public bool GetUnlocked()
    {
        return unlocked;
    }

    public void SetUnlocked(bool state)
    {
        unlocked = state;

        if (unlocked)
        {
            starImage.sprite = unlockedStarSprite;
            textboxImage.sprite = textboxImageUnlocked;
            textboxMarker.sprite = textboxMarkerUnlocked;
        }
        else
        {
            starImage.sprite = lockedStarSprite;
            textboxImage.sprite = textboxImageLocked;
            textboxMarker.sprite = textboxMarkerLocked;
        }
    }

    public void SetUnlockDistance(int value)
    {
        unlockDistance = value;
        distanceText.text = unlockDistance.ToString("n0") + " m";
    }

    public void DisplayTextbox(bool state)
    {
        if (state == true)
        {
            textbox.SetActive(true);
        }
        else{
            textbox.SetActive(false);
        }
    }
}
