using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class RollParticles : MonoBehaviour
{
    [SerializeField] Transform scaleAnchor;

    ParticleSystem rollParticleSystem;
    SnowballSizeManager snowballSizeManager;
    GameManager gameManager;

    bool isLosingSnow;
    float initialRateOverTime;
    float initialParticleScale;

    void Start()
    {
        rollParticleSystem = GetComponent<ParticleSystem>();
        snowballSizeManager = GetComponentInParent<SnowballSizeManager>();
        gameManager = FindObjectOfType<GameManager>();

        var emission = rollParticleSystem.emission;
        initialRateOverTime = emission.rateOverTime.Evaluate(0);

        var mainModule = rollParticleSystem.main;
        initialParticleScale = mainModule.startSize.Evaluate(0);
        mainModule.startSpeed = 1f;
    }

    void Update()
    {
        var emission = rollParticleSystem.emission;
        var mainModule = rollParticleSystem.main;
        
        // Ensure the particles move at the same speed as the background scrolling
        float simulationSpeed = gameManager.downhillSpeed;
        mainModule.simulationSpeed = simulationSpeed;

        isLosingSnow = snowballSizeManager.GetCurrentSizeRate() < 0f;

        if (isLosingSnow)
        {
            // Ensure rate over time is consistent
            emission.rateOverTime = initialRateOverTime / simulationSpeed;
            mainModule.startSize = initialParticleScale * scaleAnchor.localScale.x;
        }
        else
        {
            emission.rateOverTime = 0f;
        }
    }
}
