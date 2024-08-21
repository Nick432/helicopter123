using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    [SerializeField] Sprite unequippedIcon;
    [SerializeField] float minFlashAlpha;
    [SerializeField] float period = 2f;
    public int slotIndex;

    Image image;

    Coroutine flashingCoroutine;

    bool awaitingSelection;
    bool highlighted;

    ShopItem equippedShopItem;
    ShopItem potentialShopItem;

    string slotName;

    void Awake()
    {
        image = GetComponent<Image>();

        slotName = transform.parent.name;
    }

    void StopAwaitingSelection()
    {
        if (!awaitingSelection) return;
        awaitingSelection = false;

        Color originalColour = image.color;
        originalColour.a = 1f;
        image.color = originalColour;

        ShopPlayerInput.OnClickEvent -= StopAwaitingSelection;
        if (flashingCoroutine != null)
        {
            StopCoroutine(flashingCoroutine);
            flashingCoroutine = null;
        }

        if (!highlighted) return;

        if (equippedShopItem != null)
        {
            equippedShopItem.Replaced();
        }
        equippedShopItem = potentialShopItem;
        
        image.sprite = equippedShopItem.itemIcon.GetComponent<Image>().sprite;
        equippedShopItem.SuccessfullyEquipped(slotName, slotIndex);
    }

    public void MouseOverIcon(bool state)
    {
        highlighted = state;
    }

    void AwaitSelection(ShopItem shopItem)
    {
        if (awaitingSelection) return;

        awaitingSelection = true;
        potentialShopItem = shopItem;

        ShopPlayerInput.OnClickEvent += StopAwaitingSelection;

        flashingCoroutine = StartCoroutine(FlashContinuously());
    }

    public void Unequip(ShopItem shopItem)
    {
        Sprite unequipSprite = shopItem.itemIcon.GetComponent<Image>().sprite;
        if (unequipSprite == image.sprite)
        {
            image.sprite = unequippedIcon;
            equippedShopItem = null;
        }
    }

    public void EquipFromSave(ShopItem shopItem)
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        image.sprite = shopItem.itemIcon.GetComponent<Image>().sprite;
        equippedShopItem = shopItem;
    }

    IEnumerator FlashContinuously()
    {
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            float cycles = time / period;

            const float tau = Mathf.PI * 2f;
            float angle = cycles * tau - (Mathf.PI / 2f);

            float rawValue = Mathf.Sin(angle);

            float value = (rawValue + 1f) / 2f;
            float alpha = value * (1f - minFlashAlpha);

            Color newColour = image.color;
            newColour.a = 1f - alpha;
            image.color = newColour;

            yield return new WaitForEndOfFrame();
        }
    }
        

    void OnEnable() 
    {
        ShopItem.OnEquippingItem += AwaitSelection;
        ShopItem.OnUnequipItem += Unequip;
    }

    void OnDisable()
    {
        ShopItem.OnEquippingItem -= AwaitSelection;
        ShopItem.OnUnequipItem -= Unequip;
        ShopPlayerInput.OnClickEvent -= StopAwaitingSelection;
    }
}
