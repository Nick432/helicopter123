using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class fuel_meter : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] [Tooltip("Maximum fuel by Seconds of usage")] float fuelTankCapacity = 30f;

    [SerializeField] OverlayCanvas overlayCanvas;

    float fuel;
    float fuelTankPercentage;

    bool gameOver;

    void Start()
    {
        fuel = fuelTankCapacity;
    }

    void Update()
    {
        if (gameOver) return;
        
        HandleFuelDepletion();
        DrawFuelToUI();
    }

    void HandleFuelDepletion()
    {
        if (fuel > 0f)
        {
            fuel -= Time.deltaTime;
        }
        else
        {
            fuel = 0f;
            gameOver = true;

            Debug.Log("Out of fuel!!!");

            // Game over logic
        }
    }
    
    void DrawFuelToUI()
    {
        fuelTankPercentage = fuel / fuelTankCapacity;
        overlayCanvas.DrawFuelGauge(fuelTankPercentage);
    }

    public void AddFuel(float value)
    {
        fuel += value;
        fuel = Mathf.Clamp(fuel, float.MinValue, fuelTankCapacity);
    }
}
