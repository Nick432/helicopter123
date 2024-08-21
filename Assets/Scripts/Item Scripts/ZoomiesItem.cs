using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomiesItem : MonoBehaviour
{
    public List<float> moveSpeeds = new List<float>();
    public List<float> dragGradients = new List<float>();

    ItemScriptManager itemScriptUpgradeLevel;
    Snowball snowball;
    PlayerAgility playerAgility;

    int upgradeLevel;
    
    void Awake()
    {
        itemScriptUpgradeLevel = GetComponent<ItemScriptManager>();
    }

    void OnGameStart()
    {
        snowball = FindObjectOfType<Snowball>();
        playerAgility = FindObjectOfType<PlayerAgility>();

        if (snowball == null) return;
        if (playerAgility == null) return;

        upgradeLevel = itemScriptUpgradeLevel.upgradeLevel;

        snowball.moveSpeed = moveSpeeds[upgradeLevel - 1];
        playerAgility.gradient = dragGradients[upgradeLevel - 1];
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
