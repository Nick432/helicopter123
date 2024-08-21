using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;

    PlayerUpgradesManager playerUpgradesManager;

    void Awake()
    {
        playerUpgradesManager = FindObjectOfType<PlayerUpgradesManager>();

        DrawMoney();
    }

    public void DrawMoney()
    {
        moneyText.text = playerUpgradesManager.money.ToString("n0");
    }
}
