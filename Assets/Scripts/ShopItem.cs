using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    [SerializeField] GameObject itemScriptPrefab;
    [SerializeField] Type type;
    [Header("Equipping")]
    [SerializeField] GameObject equipButton;
    public GameObject itemIcon;
    [SerializeField] Sprite equippableSprite;
    [SerializeField] Sprite equippableHighlightedSprite;
    [SerializeField] Sprite unequippableSprite;
    [SerializeField] Sprite unequippableHighlightedSprite;
    [SerializeField] TextMeshProUGUI equipText;
    [SerializeField] Transform slotLayoutGroup;

    enum Type
    {
        item,
        ability,
    }

    Image equipButtonImage;

    ShopElementInfo savedInfo;
    PlayerUpgradesManager playerUpgradesManager;
    Transform itemScriptsGroup;
    Transform abilityScriptsGroup;

    ShopElement shopElement;

    string itemScriptInstanceName;
    string equippedSlotName;

    int upgradeLevel;
    int abilityIndex;

    bool equipped;
    bool highlighted;

    public static event Action<ShopItem> OnEquippingItem;
    public static event Action<ShopItem> OnUnequipItem;

    void Start()
    {
        playerUpgradesManager = FindObjectOfType<PlayerUpgradesManager>();
        itemScriptsGroup = playerUpgradesManager.itemScriptsGroup;
        abilityScriptsGroup = playerUpgradesManager.abilityScriptsGroup;
        shopElement = GetComponent<ShopElement>();
        equipButtonImage = equipButton.GetComponent<Image>();

        RetreiveSavedInfo();
        upgradeLevel =  shopElement.upgradeLevel;

        SetUpEquipButton();
        UpdateEquipButtonSprite();
    }

    void RetreiveSavedInfo()
    {
        bool hasSavedData = playerUpgradesManager.shopInfo.shopElementInfos.TryGetValue(name,
                                                                        out savedInfo);
        if (hasSavedData)
        {
            equipped = savedInfo.equipped;
            shopElement.upgradeLevel = savedInfo.upgradeLevel;
            itemScriptInstanceName = savedInfo.itemScriptInstanceName;
            equippedSlotName = savedInfo.equippedSlotName;
            abilityIndex = savedInfo.abilityIndex;

            if (equippedSlotName == null) return;

            Transform[] equipSlotTransforms = slotLayoutGroup.GetComponentsInChildren<Transform>();
            foreach (Transform equipSlotTransform in equipSlotTransforms)
            {
                if (equipSlotTransform.name == equippedSlotName)
                {
                    equipSlotTransform.GetComponentInChildren<EquipSlot>().EquipFromSave(this);
                }
            }
        }
        else
        {
            savedInfo = new ShopElementInfo();
            playerUpgradesManager.shopInfo.shopElementInfos.Add(name, savedInfo);
        }
    }

    void SetUpEquipButton()
    {
        if (upgradeLevel > 0)
        {
            equipButton.SetActive(true);
        }
        else
        {
            equipButton.SetActive(false);
        }
    }

    public void HighlightEquipButton(bool state)
    {
        highlighted = state;
        UpdateEquipButtonSprite();
    }

    public void UpgradeItem(int newUpgradeLevel)
    {
        upgradeLevel = newUpgradeLevel;

        SetUpEquipButton();

        if (equipped)
        {
            HandleItemScriptEquipping();
        }
    }

    public void SuccessfullyEquipped(string slotName, int slotIndex)
    {
        equipped = true;
        equippedSlotName = slotName;
        if (type == Type.ability)
        {
            abilityIndex = slotIndex;
        }

        UpdateEquipButtonSprite();
        HandleItemScriptEquipping();
    }

    void UpdateEquipButtonSprite()
    {
        if (equipped && highlighted)
        {
            equipButtonImage.sprite = unequippableHighlightedSprite;
        }
        else if (equipped && !highlighted)
        {
            equipButtonImage.sprite = unequippableSprite;
        }
        else if (!equipped && highlighted)
        {
            equipButtonImage.sprite = equippableHighlightedSprite;
        }
        else
        {
            equipButtonImage.sprite = equippableSprite;
        }

        if (equipped)
        {
            equipText.text = "REMOVE";
        }
        else
        {
            equipText.text = "EQUIP";
        }
    }

    public void OnClickEquipButton()
    {
        if (equipped)
        {
            OnUnequipItem?.Invoke(this);
            equipped = false;

            UpdateEquipButtonSprite();
            HandleItemScriptEquipping();
        }
        else
        {
            OnEquippingItem?.Invoke(this);
        }
    }

    public void Replaced()
    {
        equipped = false;
        UpdateEquipButtonSprite();
        HandleItemScriptEquipping();
    }

    void HandleItemScriptEquipping()
    {
        bool equipping = equipped;

        Transform parent = itemScriptsGroup;
        if (type == Type.ability)
        {
            parent = abilityScriptsGroup;
        }

        // Destroy any existing instances regardless of whether re-equipping or not.
        Transform[] itemScriptsTransforms = parent.GetComponentsInChildren<Transform>();
        foreach(Transform itemScriptTransform in itemScriptsTransforms)
        {
            if (itemScriptTransform.gameObject.name == itemScriptInstanceName)
            {
                Destroy(itemScriptTransform.gameObject);
                if (!equipping)
                {
                    equippedSlotName = null;
                    abilityIndex = 0;
                }
                itemScriptInstanceName = null;
            }
        }
        
        // Equip new one if applicable.
        if (equipping)
        {
            GameObject instance = Instantiate(itemScriptPrefab, parent.position, 
                                             quaternion.identity, parent);
            itemScriptInstanceName = instance.name;
            ItemScriptManager scriptUpgradeLevel = instance.GetComponent<ItemScriptManager>();
            scriptUpgradeLevel.upgradeLevel = upgradeLevel;
            if (type == Type.ability)
            {
                scriptUpgradeLevel.abilityIndex = abilityIndex;
            }
        }
    }

    void OnDisable()
    {
        SaveInfo();
    }

    void SaveInfo()
    {
        savedInfo.upgradeLevel = upgradeLevel;
        savedInfo.equipped = equipped;
        savedInfo.itemScriptInstanceName = itemScriptInstanceName;
        savedInfo.equippedSlotName = equippedSlotName;
        savedInfo.abilityIndex = abilityIndex;

        if (playerUpgradesManager == null) return;
        playerUpgradesManager.shopInfo.shopElementInfos[name] = savedInfo;
    }
}
