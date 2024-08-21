using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShopElementInfo
{
    // All
    public int upgradeLevel;

    // Enhancements
    public int selectedSegmentIndex;

    // Items & Abilities
    public bool equipped;
    public string itemScriptInstanceName;
    public string equippedSlotName;
    public int abilityIndex;
}
