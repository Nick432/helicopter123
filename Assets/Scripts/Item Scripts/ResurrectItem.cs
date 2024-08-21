using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectItem : MonoBehaviour
{
    public List<float> resurrectSizes = new List<float>();
    public List<float> immunityTimes = new List<float>();

    ItemScriptManager itemScriptUpgradeLevel;
    SnowballSizeManager snowballSizeManager;

    int upgradeLevel;
    
    void Awake()
    {
        itemScriptUpgradeLevel = GetComponent<ItemScriptManager>();
    }

    void OnGameStart()
    {
        snowballSizeManager = FindObjectOfType<SnowballSizeManager>();
        if (snowballSizeManager == null) return;

        upgradeLevel = itemScriptUpgradeLevel.upgradeLevel;

        snowballSizeManager.resurrectOnDeath = true;
        snowballSizeManager.resurrectSize = resurrectSizes[upgradeLevel - 1];
        snowballSizeManager.resurrectImmunityTime = immunityTimes[upgradeLevel - 1];
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
