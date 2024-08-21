using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TextMeshProUGUI totalText;

    public void DisplayCanvas(bool state)
    {
        canvas.SetActive(state);
        if (state == true)
        {
            int money = FindObjectOfType<PlayerUpgradesManager>().money;

            totalText.text = ": " + money.ToString("n0");
        }
    }
}
