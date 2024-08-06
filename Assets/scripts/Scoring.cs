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
    //[SerializeField] int speedIncreaseDistanceInterval = 50;

    OverlayCanvas overlayCanvas;

    GameManager gameManager;

    [HideInInspector] public float trueDistance; // The precise distance travelled.
    int scoreDistance;  // The distance travelled rounded to an interval of the scoreInterval.

    public static event Action<int> OnUpdatedScore;

    bool gameOver;

    void Awake()
    {
        overlayCanvas = FindObjectOfType<OverlayCanvas>();
        gameManager = FindObjectOfType<GameManager>();
        trueDistance = startDistance;
    }

    void Start()
    {
        overlayCanvas.DrawDistanceScore(scoreDistance);
        OnUpdatedScore?.Invoke(scoreDistance);
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
        if (currentScoreDistance != scoreDistance)
        {
            overlayCanvas.DrawDistanceScore(scoreDistance);
            
            OnUpdatedScore?.Invoke(scoreDistance);

            if (scoreDistance > gameManager.highScore)
            {
                gameManager.highScore = scoreDistance;
                overlayCanvas.DrawHighScore(gameManager.highScore);
            }

            // if (scoreDistance % speedIncreaseDistanceInterval == 0)
            // {
            //     gameManager.IncreaseDownhillSpeed();
            // }
        }
    }

    void HandleGameOver()
    {
        gameOver = true;
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
