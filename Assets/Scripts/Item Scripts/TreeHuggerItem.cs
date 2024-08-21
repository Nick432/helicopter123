using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHuggerItem : MonoBehaviour
{
    public List<float> resistances = new List<float>();
    public List<float> treeCashChances = new List<float>();
    public List<int> treeCashAmounts = new List<int>();

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

        snowballSizeManager.treeReduction = resistances[upgradeLevel - 1];
        snowballSizeManager.chanceForTreeCash = treeCashChances[upgradeLevel - 1];
        snowballSizeManager.treeCashAmount = treeCashAmounts[upgradeLevel - 1];
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
