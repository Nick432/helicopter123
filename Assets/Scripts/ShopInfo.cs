using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Info", menuName = "New Shop Info")]
public class ShopInfo : ScriptableObject
{
    public Dictionary<string, ShopElementInfo> shopElementInfos = 
                                    new Dictionary<string, ShopElementInfo>();
    public Dictionary<string, EquipSlotInfo> equipSlotInfos = 
                                    new Dictionary<string, EquipSlotInfo>();
}
