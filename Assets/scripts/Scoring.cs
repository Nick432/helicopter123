using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class Scoring : MonoBehaviour
{
    [SerializeField] int metersBySpeedFactor = 50;
    [Tooltip("Score is calculated in intervals of this number")]
    [SerializeField] int scoreInterval = 50;
    [SerializeField] float startDistance;

    OverlayCanvas overlayCanvas;

    Game_Manager gameManager;

    float trueDistance; // The precise distance travelled.
    int scoreDistance;  // The distance travelled rounded to an interval of the scoreInterval.

    public static event Action<int> OnUpdatedScore;

    bool gameOver;

    void Awake()
    {
        overlayCanvas = FindObjectOfType<OverlayCanvas>();
        gameManager = FindObjectOfType<Game_Manager>();
        trueDistance = startDistance;
    }
    
    void Update()
    {
        if (gameOver) return;

        HandleDistanceTravelled();
    }

    void HandleDistanceTravelled()
    {
        float speed = gameManager.downhillSpeed;

        // Store current score.
        int currentScoreDistance = scoreDistance;

        // Update distance score.
        trueDistance += speed * metersBySpeedFactor * Time.deltaTime;
        scoreDistance = (int)(trueDistance / scoreInterval) * scoreInterval;

        // Draw score to UI when it changes value.
        if (currentScoreDistance != trueDistance)
        {
            overlayCanvas.DrawDistanceScore(scoreDistance);
            
            OnUpdatedScore?.Invoke(scoreDistance);
        }
    }

    void HandleGameOver()
    {
        gameOver = true;

        int highScore = gameManager.highScore;

        if (scoreDistance > highScore)
        {
            gameManager.highScore = scoreDistance;
            overlayCanvas.DrawHighScore(highScore);
        }
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
