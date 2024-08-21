using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class Scoring : MonoBehaviour
{
    [SerializeField] float metersBySpeedFactor = 50;
    [Tooltip("Score is calculated in intervals of this number")]
    [SerializeField] int scoreInterval = 50;
    [SerializeField] float startDistance;

    OverlayCanvas overlayCanvas;

    GameManager gameManager;
    DownhillSpeedManager downhillSpeedManager;

    [HideInInspector] public float trueDistance; // The precise distance travelled.
    int scoreDistance;  // The distance travelled rounded to an interval of the scoreInterval.

    public static event Action<int> OnUpdatedScore;

    bool gameOver;

    int coinsCollectedThisRun;

    void Awake()
    {
        overlayCanvas = FindObjectOfType<OverlayCanvas>();
        gameManager = FindObjectOfType<GameManager>();
        downhillSpeedManager = FindObjectOfType<DownhillSpeedManager>();
        trueDistance = startDistance;
    }

    void Start()
    {
        overlayCanvas.DrawDistanceScore(scoreDistance);
        overlayCanvas.DrawCollectedCoins(coinsCollectedThisRun);
        OnUpdatedScore?.Invoke(scoreDistance);
    }
    
    void Update()
    {
        if (gameOver) return;

        HandleDistanceTravelled();
    }

    void HandleDistanceTravelled()
    {
        float speed = downhillSpeedManager.downhillSpeed;

        // Store current score.
        int currentScoreDistance = scoreDistance;

        // Update distance score.
        trueDistance += speed * metersBySpeedFactor * Time.deltaTime;
        scoreDistance = (int)(trueDistance / scoreInterval) * scoreInterval;

        // Draw score to UI when it changes value.
        if (currentScoreDistance != scoreDistance)
        {
            overlayCanvas.DrawDistanceScore(scoreDistance);
            
            OnUpdatedScore?.Invoke(scoreDistance);

            if (scoreDistance > gameManager.highScore)
            {
                gameManager.highScore = scoreDistance;
                overlayCanvas.DrawHighScore(gameManager.highScore);
            }
        }
    }

    void HandleGameOver()
    {
        gameOver = true;
        FindObjectOfType<PlayerUpgradesManager>().money += coinsCollectedThisRun;
    }

    public void AddCoins(int amount)
    {
        coinsCollectedThisRun += amount;
        overlayCanvas.DrawCollectedCoins(coinsCollectedThisRun);
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
