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
    [Header("References")]
    [SerializeField] GameObject expandedMenu;
    [SerializeField] RectTransform layoutGroupRect;
    [SerializeField] GameObject dropdownArrow;
    [SerializeField] TextMeshProUGUI nextUpgradeDescription;
    [SerializeField] List<string> nextUpgradeDescriptions = new List<string>();
    [SerializeField] bool ignoreNextUpgradeDescriptions;

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
    ShopCanvas shopCanvas;

    Image purchaseButtonImage;
    Button purchaseButtonButton;
    Image[] upgradeSegmentImages;

    bool mainButtonSelected;
    bool mainButtonHighlighted;

    [HideInInspector] public int upgradeLevel;
    int maxUpgradeLevel = 1;

    public static event Action OnExpandedMenuClicked;
    public static event Action OnUpdateShopPurchasability;
    public UnityEvent<int> OnNewUpgrade;

    void Awake()
    {
        playerUpgradesManager = FindObjectOfType<PlayerUpgradesManager>();
        rectTransform = GetComponent<RectTransform>();
        shopCanvas = FindObjectOfType<ShopCanvas>();

        purchaseButtonImage = purchaseButton.GetComponent<Image>();
        purchaseButtonButton = purchaseButton.GetComponent<Button>();
    }

    void Start()
    {
        playerUpgradesManager = FindObjectOfType<PlayerUpgradesManager>();

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

        for (int i = 0; i < upgradeLevel; i++)
        {
            upgradeSegmentImages[i].sprite = upgradedSprite;
        }
        OnUpdateShopPurchasability?.Invoke();

        OnNewUpgrade.Invoke(upgradeLevel);
    }

    void UpdatePurchaseInformation()
    {
        TextMeshProUGUI purchaseButtonText = purchaseButton.GetComponentInChildren<TextMeshProUGUI>();
        // Set cost text
        if (upgradeLevel == maxUpgradeLevel)
        {
            costText.text = "";
            purchaseButtonText.text = "MAX";
            if (!ignoreNextUpgradeDescriptions)
            {
                nextUpgradeDescription.text = "";
            }
            purchaseButtonImage.sprite = unpurchasableSprite;
            purchaseButtonButton.interactable = false;
            return;
        }
        if (upgradeLevel > 0)
        {
            purchaseButtonText.text = "UPGRADE";
            if (upgradeLevel < maxUpgradeLevel && !ignoreNextUpgradeDescriptions)
            {
                nextUpgradeDescription.text = nextUpgradeDescriptions[upgradeLevel - 1];
            }
        }

        if (upgradeLevel >= maxUpgradeLevel) return;

        int nextUpgradeCost = upgradeCosts[upgradeLevel];
        costText.text = nextUpgradeCost.ToString("n0");

        // Set purchase button
        if (playerUpgradesManager.money >= nextUpgradeCost)
        {
            purchaseButtonImage.sprite = purchasableSprite;
            purchaseButtonButton.interactable = true;
            costText.color = Color.black;
        }
        else
        {
            purchaseButtonImage.sprite = unpurchasableSprite;
            purchaseButtonButton.interactable = false;
            costText.color = Color.red;
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
                                                              -90f);
            
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
        shopCanvas.DrawMoney();

        upgradeLevel++;

        // Set upgrade bar sprites.
        for (int i = 0; i < upgradeLevel; i++)
        {
            upgradeSegmentImages[i].sprite = upgradedSprite;
        }

        // Update purchase info on all shop elements.
        OnUpdateShopPurchasability?.Invoke();

        OnNewUpgrade.Invoke(upgradeLevel);
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
