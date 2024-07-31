using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    [Header("Global Move Speed")]
    public float topDownhillSpeed = 5f;
    [SerializeField] AnimationCurve downhillSpeedBySizeCurve;
    [SerializeField] float topDownhillSpeedIncreasePerWave;
    [SerializeField] float downhillSpeedTransitionRate;

    [HideInInspector] public float downhillSpeed;
    [HideInInspector] public float initialDownhillSpeed;

    [Header("Game Over")]
    [SerializeField] float reloadDelay;

    SnowballSizeManager snowballSizeManager;

    [HideInInspector] public int highScore;

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
        topDownhillSpeed = initialDownhillSpeed;
    }

    // Don't destroy the gameManager on load. This means certain variables are not reset when
    // the game resets.
    void ManageSingleton()
    {
        int instanceCount = FindObjectsOfType<Game_Manager>().Length;

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
        float targetSpeed = topDownhillSpeed * downhillSpeedBySizeCurve.Evaluate(sizePercentage);
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
        topDownhillSpeed = 0f;
        StartCoroutine(ReloadGame());
    }

    IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(reloadDelay);

        SceneManager.LoadScene(0);
    }

    public void IncreaseTopDownhillSpeed()
    {
        topDownhillSpeed += topDownhillSpeedIncreasePerWave;
        //StartCoroutine(TransitionMoveSpeed());
    }

    IEnumerator TransitionMoveSpeed()
    {
        float currentMoveSpeed = topDownhillSpeed;
        float targetMoveSpeed = topDownhillSpeed + topDownhillSpeedIncreasePerWave;
        while (currentMoveSpeed < targetMoveSpeed)
        {
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, targetMoveSpeed,
                                                 downhillSpeedTransitionRate * Time.deltaTime);
            topDownhillSpeed = currentMoveSpeed;

            yield return new WaitForEndOfFrame();
        }
        topDownhillSpeed = targetMoveSpeed;
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
