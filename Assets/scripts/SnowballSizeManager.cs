using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System;

public class SnowballSizeManager : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 1f)] float initialSize = 0.01f;

    OverlayCanvas overlayCanvas;

    float sizePercentage;

    float currentSizeRate;
    float passiveSizeRate;
    float overrideSizeRate;
    bool useOverrideSizeRate;

    List<ApplySizeRate> applySizeRates = new List<ApplySizeRate>();

    public static event Action OnGameOver;

    bool gameOver;

    void Awake()
    {
        overlayCanvas = FindObjectOfType<OverlayCanvas>();
    }

    void Start()
    {
        sizePercentage = initialSize;
    }

    void UpdateCurrentSizeRate()
    {
        if (useOverrideSizeRate)
        {
            currentSizeRate = overrideSizeRate;
        }
        else
        {
            currentSizeRate = passiveSizeRate;
        }
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

            OnGameOver?.Invoke();
        }
    }
    
    void DrawSizeToUI()
    {
        overlayCanvas.DrawSizeMeter(sizePercentage);
    }

    // Used by background manager to change the passiveSizeRate.
    void SetPassiveSizeRate(float rate)
    {
        passiveSizeRate = rate;
        UpdateCurrentSizeRate();
    }

    public void AddSizePercentage(float value)
    {
        sizePercentage += value;
        sizePercentage = Mathf.Clamp(sizePercentage, 0f, 1f);
    }

    public float GetSizePercentage()
    {
        return sizePercentage;
    }

    public float GetCurrentSizeRate()
    {
        return currentSizeRate;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        ApplySizeRate applySizeRate = other.GetComponentInParent<ApplySizeRate>();

        if (applySizeRate != null)
        {
            applySizeRates.Add(applySizeRate);
            HandleApplySizeRates();
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        ApplySizeRate applySizeRate = other.GetComponentInParent<ApplySizeRate>();

        if (applySizeRate != null)
        {
            applySizeRates.Remove(applySizeRate);
            HandleApplySizeRates();
        }
    }

    void HandleApplySizeRates()
    {
        if (applySizeRates.Count == 0)
        {
            useOverrideSizeRate = false;
        }
        else
        {
            // Prioritise using the largest rate if multiple are set simultaneously.
            // Order list by largest sizeRateOnContact to smallest.
            applySizeRates = applySizeRates.OrderByDescending(x => x.sizeRateOnContact).ToList();
            // Use the largest rate.
            overrideSizeRate = applySizeRates[0].sizeRateOnContact;

            useOverrideSizeRate = true;
        }
        
        UpdateCurrentSizeRate();
    }

    void OnEnable() 
    {
        BackgroundManager.OnSetPassiveSizeRate += SetPassiveSizeRate;
    }

    void OnDisable()
    {
        BackgroundManager.OnSetPassiveSizeRate -= SetPassiveSizeRate;
    }
}
