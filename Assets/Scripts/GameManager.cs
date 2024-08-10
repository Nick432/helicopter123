using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Downhill Speed")]
    public float topDownhillSpeed = 5f;
    [SerializeField] AnimationCurve downhillSpeedBySizeCurve;
    [SerializeField] float topSpeedIncreaseAmount;
    [SerializeField] float topSpeedIncreaseDelay;
    [SerializeField] float downhillSpeedTransitionRate;

    [HideInInspector] public float downhillSpeed;
    [HideInInspector] public float initialDownhillSpeed;

    [Header("Game Over")]
    [SerializeField] float reloadDelay;

    SnowballSizeManager snowballSizeManager;
    GameOverCanvas gameOverCanvas;

    Coroutine increaseSpeedCoroutine;

    // Stored values across runs.

    [HideInInspector] public int highScore;
    [HideInInspector] public bool[] starUnlockStates = new bool[3];

    void Awake()
    {
        initialDownhillSpeed = topDownhillSpeed;
    }

    void Start()
    {
        ManageSingleton();
    }

    public void HandleOnGameStart()
    {
        snowballSizeManager = FindObjectOfType<SnowballSizeManager>();
        gameOverCanvas = FindObjectOfType<GameOverCanvas>();
        
        topDownhillSpeed = initialDownhillSpeed;

        increaseSpeedCoroutine = StartCoroutine(ContinuouslyIncreaseSpeed());
    }

    // Don't destroy the gameManager on load. This means certain variables are not reset when
    // the game resets.
    void ManageSingleton()
    {
        int instanceCount = FindObjectsOfType<GameManager>().Length;

        //If there is more than one GameManager, destroy the newest one.
        if (instanceCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
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

    IEnumerator ContinuouslyIncreaseSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(topSpeedIncreaseDelay);
            
            topDownhillSpeed += topSpeedIncreaseAmount;
        }
    }

    // public void IncreaseDownhillSpeed()
    // {
    //     topDownhillSpeed += topSpeedIncreaseAmount;
    // }

    void HandleGameOver()
    {
        topDownhillSpeed = 0f;
        StopCoroutine(increaseSpeedCoroutine);
        StartCoroutine(ReloadGame());
    }

    IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(reloadDelay);

        gameOverCanvas.DisplayCanvas(true);
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
