using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scoring : MonoBehaviour
{
    [SerializeField] int metresTravelledPerSecond = 50;
    [Tooltip("Score is calculated in intervals of this number")]
    [SerializeField] int scoreInterval = 50;

    [SerializeField] OverlayCanvas overlayCanvas;

   public float gameTime;
   public float trueDistance; // The precise distance travelled.
   public int scoreDistance;  // The distance travelled rounded to an interval of the scoreInterval.
    public int BestScoreDistance = 0;
    public static event Action<int> OnUpdatedScore;
    
    void Update()
    {
        HandleDistanceTravelled();
    }

    void HandleDistanceTravelled()
    {
        gameTime += Time.deltaTime;

        // Store current score.
        int currentScoreDistance = scoreDistance;

        // Update distance score.
        trueDistance = gameTime * metresTravelledPerSecond;
        scoreDistance = (int)(trueDistance / scoreInterval) * scoreInterval;

        // Draw score to UI when it changes value.
        if (currentScoreDistance != trueDistance)
        {
            overlayCanvas.DrawDistanceScore(scoreDistance);
            
            OnUpdatedScore?.Invoke(scoreDistance);
        }
    }
}
