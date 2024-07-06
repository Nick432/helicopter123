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
    Game_Manager gameManager;

    bool isLosingSnow;
    float initialRateOverTime;
    float initialParticleScale;

    void Start()
    {
        rollParticleSystem = GetComponent<ParticleSystem>();
        snowballSizeManager = GetComponentInParent<SnowballSizeManager>();
        gameManager = FindObjectOfType<Game_Manager>();

        var emission = rollParticleSystem.emission;
        initialRateOverTime = emission.rateOverTime.Evaluate(0);

        var mainModule = rollParticleSystem.main;
        initialParticleScale = mainModule.startSize.Evaluate(0);
    }

    void Update()
    {
        var emission = rollParticleSystem.emission;
        var mainModule = rollParticleSystem.main;

        isLosingSnow = snowballSizeManager.GetCurrentSizeRate() < 0f;

        if (isLosingSnow)
        {
            emission.rateOverTime = initialRateOverTime;
            mainModule.startSpeed = gameManager.globalBaseMoveSpeed;
            mainModule.startSize = initialParticleScale * scaleAnchor.localScale.x;
        }
        else
        {
            emission.rateOverTime = 0f;
        }
    }
}
