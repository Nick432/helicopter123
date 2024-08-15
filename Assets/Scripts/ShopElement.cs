using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopElement : MonoBehaviour
{
    [SerializeField] GameObject expandedMenu;
    [SerializeField] RectTransform layoutGroupRect;
    [SerializeField] GameObject dropdownArrow;
    // [SerializeField] GameObject player_upgrades_manager;
    [SerializeField] public int Element_Index;

    [Header("Main Button")]
    [SerializeField] Image mainButtonImage;
    [SerializeField] Sprite unselectedSprite;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Color highlightedTint;

    [Header("Purchasing")]
    [SerializeField] GameObject upgradeBarLayoutGroup;
    [SerializeField] GameObject upgradeSegmentPrefab;
    [SerializeField] Sprite upgradedSprite;
    [SerializeField] GameObject purchaseButton;
    [SerializeField] Sprite purchasableSprite;
    [SerializeField] Sprite purchasableHighlightedSprite;
    [SerializeField] Sprite unpurchasableSprite;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] List<int> upgradeCosts = new List<int>();

    float collapsedHeight;
    float expandedHeight;

    RectTransform rectTransform;
    PlayerUpgradesManager playerUpgradesManager;

    Image purchaseButtonImage;
    Button purchaseButtonButton;
    Image[] upgradeSegmentImages;

    bool mainButtonSelected;
    bool mainButtonHighlighted;

    int upgradeLevel;
    int maxUpgradeLevel;

    public static event Action OnExpandedMenuClicked;
    public static event Action OnUpdateShopPurchasability;
    public UnityEvent<int> OnNewUpgrade;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        playerUpgradesManager = FindObjectOfType<PlayerUpgradesManager>();

       for ( int i = 0;  i < 6; i++)
        {

            playerUpgradesManager.itemupgradelevels.Add(0);

        }


        purchaseButtonImage = purchaseButton.GetComponent<Image>();
        purchaseButtonButton = purchaseButton.GetComponent<Button>();
    }

    void Start()
    {
        
        upgradeLevel = playerUpgradesManager.itemupgradelevels[Element_Index];

       
        //on start set the upgrade level to the saved number in the upgrades manager

        collapsedHeight = rectTransform.rect.height;
        expandedHeight = expandedMenu.GetComponent<RectTransform>().rect.height;
        maxUpgradeLevel = upgradeCosts.Count;
        SetUpUpgradeLevelBar();
        UpdatePurchaseInformation();
    }

    void SetUpUpgradeLevelBar()
    {
        for (int i = 0; i < maxUpgradeLevel; i++)
        {
            Instantiate(upgradeSegmentPrefab, upgradeBarLayoutGroup.transform.position,
                        quaternion.identity, upgradeBarLayoutGroup.transform);
        }

        upgradeSegmentImages = upgradeBarLayoutGroup.GetComponentsInChildren<Image>();

        Debug.Log("saved over upgrade level" + upgradeLevel.ToString());
        for (int i = 0; i < upgradeLevel; i++)
        {
            Debug.Log("changed segment sprite");

            upgradeSegmentImages[i].sprite = upgradedSprite;
        }
        OnUpdateShopPurchasability?.Invoke();

        OnNewUpgrade.Invoke(upgradeLevel);
    }

    void UpdatePurchaseInformation()
    {
        // Set cost text
        int nextUpgradeCost = upgradeCosts[upgradeLevel];
        costText.text = nextUpgradeCost.ToString();

        // Set purchase button
        if (playerUpgradesManager.money >= nextUpgradeCost)
        {
            purchaseButtonImage.sprite = purchasableSprite;
            purchaseButtonButton.interactable = true;
        }
        else
        {
            purchaseButtonImage.sprite = unpurchasableSprite;
            purchaseButtonButton.interactable = false;
        }
    }

    public void ExpandedMenuClicked()
    {
        if (mainButtonSelected)
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
        mainButtonSelected = state;
        UpdateMainButtonSprite();

        if (state == true)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, expandedHeight);

            expandedMenu.SetActive(true);

            dropdownArrow.transform.eulerAngles = new Vector3(dropdownArrow.transform.rotation.x,
                                                              dropdownArrow.transform.rotation.y,
                                                              45f);
            
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, collapsedHeight);

            expandedMenu.SetActive(false);

            dropdownArrow.transform.eulerAngles = new Vector3(dropdownArrow.transform.rotation.x,
                                                              dropdownArrow.transform.rotation.y,
                                                              0f);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroupRect);
    }



    public void HighlightMainButton(bool state)
    {
        mainButtonHighlighted = state;
        UpdateMainButtonSprite();
    }

    public void HighlightPurchaseButton(bool state)
    {
        if (purchaseButtonButton.interactable)
        {
            if (state == true)
            {
                purchaseButtonImage.sprite = purchasableHighlightedSprite;
            }
            else
            {
                purchaseButtonImage.sprite = purchasableSprite;
            }
        }
    }

    void UpdateMainButtonSprite()
    {
        if (mainButtonSelected)
        {
            mainButtonImage.sprite = selectedSprite;
        }
        else
        {
            mainButtonImage.sprite = unselectedSprite;
        }

        if (mainButtonHighlighted)
        {
            mainButtonImage.color = highlightedTint;
        }
        else
        {
            mainButtonImage.color = Color.white;
        }
    }

    // Called from purchase button click.
    public void MakePurchase()
    {
        // Upgrade to next level.
        if (upgradeLevel == maxUpgradeLevel) return;

        playerUpgradesManager.money -= upgradeCosts[upgradeLevel];

        upgradeLevel++;

        playerUpgradesManager.save_upgrade_level(Element_Index,upgradeLevel);
        //playerUpgradesManager.


        // Set upgrade bar sprites.
        for (int i = 0; i < upgradeLevel; i++)
        {
            upgradeSegmentImages[i].sprite = upgradedSprite;
        }

        // Update purchase info on all shop elements.
        OnUpdateShopPurchasability?.Invoke();

        OnNewUpgrade.Invoke(upgradeLevel);



    }

    public int GetUpgradeLevel()
    {
        return upgradeLevel;
    }

    void OnEnable()
    {
        if (mainButtonSelected)
        {
            OnExpandedMenuClicked += ExpandedMenuClicked;
        }
        OnUpdateShopPurchasability += UpdatePurchaseInformation;
    }

    void OnDisable()
    {
        OnExpandedMenuClicked -= ExpandedMenuClicked;
        OnUpdateShopPurchasability -= UpdatePurchaseInformation;
    }
}
