using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashForSnowItem : MonoBehaviour
{
    [SerializeField] List<float> patchEarnDelays = new List<float>();
    [SerializeField] float reduction;

    ItemScriptManager itemScriptUpgradeLevel;
    SnowballSizeManager snowballSizeManager;

    [HideInInspector] public int upgradeLevel;

    void Awake()
    {
        itemScriptUpgradeLevel = GetComponent<ItemScriptManager>();
    }

    void OnGameStart()
    {
        snowballSizeManager = FindObjectOfType<SnowballSizeManager>();
        if (snowballSizeManager == null) return;

        upgradeLevel = itemScriptUpgradeLevel.upgradeLevel;

        snowballSizeManager.snowPatchAndPileReduction = reduction;
        snowballSizeManager.earnMoneyFromSnowPatch = true;
        snowballSizeManager.earnDelay = patchEarnDelays[upgradeLevel - 1];
    }

    void OnEnable()
    {
        Snowball.OnGameStart += OnGameStart;
    }

    void OnDisable()
    {
        Snowball.OnGameStart -= OnGameStart;
    }
}
