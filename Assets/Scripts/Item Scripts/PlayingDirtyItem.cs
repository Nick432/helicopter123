using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingDirtyItem : MonoBehaviour
{
    public List<float> resistances = new List<float>();

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

        snowballSizeManager.dirtReduction = resistances[upgradeLevel - 1];
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
