using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnowballSizeManager : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 1f)] float initialSize;

    [SerializeField] OverlayCanvas overlayCanvas;

    float sizePercentage;
    float currentSizeRate;
    float passiveSizeRate;

    bool gameOver;

    void Start()
    {
        sizePercentage = initialSize;
        UpdateCurrentSizeRate();
    }

    void UpdateCurrentSizeRate()
    {
        currentSizeRate = passiveSizeRate;
    }

    void Update()
    {
        if (gameOver) return;
        
        HandleSizeChange();
        DrawSizeToUI();
    }

    void HandleSizeChange()
    {
        if (sizePercentage > 0f)
        {
            sizePercentage += currentSizeRate * Time.deltaTime;
            sizePercentage = Mathf.Clamp(sizePercentage, 0f, 1f);
        }
        else
        {
            sizePercentage = 0f;
            gameOver = true;

            Debug.Log("Out of snow!!!");

            // Game over logic
        }
    }
    
    void DrawSizeToUI()
    {
        overlayCanvas.DrawFuelGauge(sizePercentage);
    }

    public void AddSizePercentage(float value)
    {
        sizePercentage += value;
        sizePercentage = Mathf.Clamp(sizePercentage, 0f, 1f);
    }

    // Used by background manager to change the passiveSizeRate.
    public void SetPassiveSizeRate(float rate)
    {
        passiveSizeRate = rate;
        UpdateCurrentSizeRate();
    }

    public float GetSizePercentage()
    {
        return sizePercentage;
    }
}
