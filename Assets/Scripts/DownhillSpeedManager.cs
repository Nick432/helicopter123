using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownhillSpeedManager : MonoBehaviour
{
    public float topDownhillSpeed = 5f;
    [SerializeField] AnimationCurve downhillSpeedBySizeCurve;
    [SerializeField] float topSpeedIncreaseAmount;
    [SerializeField] float topSpeedIncreaseDelay;
    [SerializeField] float downhillSpeedTransitionRate;

    [HideInInspector] public float downhillSpeed;
    [HideInInspector] public float initialDownhillSpeed;

    SnowballSizeManager snowballSizeManager;

    Coroutine increaseSpeedCoroutine;

    void Awake()
    {
        initialDownhillSpeed = topDownhillSpeed;

        snowballSizeManager = FindObjectOfType<SnowballSizeManager>();
        
        increaseSpeedCoroutine = StartCoroutine(ContinuouslyIncreaseSpeed());

    }

    IEnumerator ContinuouslyIncreaseSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(topSpeedIncreaseDelay);
            
            topDownhillSpeed += topSpeedIncreaseAmount;
        }
    }

    void Update()
    {
        TransitionDownhillSpeedSmoothly();
    }

    void TransitionDownhillSpeedSmoothly()
    {
        float sizePercentage = snowballSizeManager.GetSizePercentage();
        float sizeDifferencePercentage = snowballSizeManager.sizeDifferenceFromDefaultPercentage;

        float sizePercentageFromDefault = sizePercentage * sizeDifferencePercentage;

        float targetSpeed = topDownhillSpeed * downhillSpeedBySizeCurve.Evaluate(sizePercentageFromDefault);

        // If the player has a max size larger than default, then increase the downhill speed
        // beyond the set number for 'topDownhillSpeed'.
        if (sizePercentageFromDefault > 1f)
        {
            float x = sizePercentageFromDefault;
            float pointToEvaluateGradient = 0.9f;
            float rise = downhillSpeedBySizeCurve.Evaluate(1f) - 
                         downhillSpeedBySizeCurve.Evaluate(pointToEvaluateGradient);
            float run = 1f - pointToEvaluateGradient;
            float gradient = rise / run;
            
            float excessiveSpeedFactor = gradient * x + 1f;
            targetSpeed = topDownhillSpeed * excessiveSpeedFactor;
        }

        // If the speed is increasing, slowly transition it to that speed. (You speed up gradually)
        if (targetSpeed > downhillSpeed)
        {
            float rate = downhillSpeedTransitionRate * Time.deltaTime;
            downhillSpeed = Mathf.MoveTowards(downhillSpeed, targetSpeed, rate);
        }
        // Otherwise, set it straight to the lower value. (You slow down immediately)
        else
        {
            downhillSpeed = targetSpeed;
        }
    }

    void HandleGameOver()
    {
        StopCoroutine(increaseSpeedCoroutine);
    }

    void OnEnable() 
    {
        SnowballSizeManager.OnGameOver += HandleGameOver;
    }

    void OnDisable()
    {
        SnowballSizeManager.OnGameOver -= HandleGameOver;
    }
}
