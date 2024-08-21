using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPileEarnings : MonoBehaviour
{
    [SerializeField] List<int> collisionAmounts = new List<int>();

    CashForSnowItem cashForSnowItem;

    void Awake()
    {
        cashForSnowItem = FindObjectOfType<CashForSnowItem>();
    }

    public void RewardCollisionMoney()
    {
        if (cashForSnowItem == null) return;

        int collisionAmountIndex = cashForSnowItem.upgradeLevel - 1;

        if (collisionAmountIndex < 0 || collisionAmountIndex >= collisionAmounts.Count) return;
        
        FindObjectOfType<Scoring>().AddCoins(collisionAmounts[collisionAmountIndex]);
    }
}
