using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressManager : MonoBehaviour
{
    [Header("Stars")]
    [SerializeField] LevelStar[] levelStars = new LevelStar[3];
    [SerializeField] int[] starDistances = new int[3];
    [SerializeField] [Range(0f, 1f)] float star3SliderValue = 0.9f;
    [SerializeField] [Range(0f, 1f)] float maxProgressValue = 0.95f; 
    [SerializeField] float transitionToMaxValueSpeed = 1f;

    OverlayCanvas overlayCanvas;
    Scoring scoring;
    GameManager gameManager;

    bool hasSurpassedStar3 = false;
    bool bestDistanceValueAtMax;

    void Awake()
    {
        overlayCanvas = FindObjectOfType<OverlayCanvas>();
        scoring = FindObjectOfType<Scoring>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        SetUpLevelStars();

        // Draw best distance to progress bar
        float bestDistanceProgressValue = gameManager.highScore / (float)starDistances[2] * star3SliderValue;
        if (bestDistanceProgressValue > star3SliderValue)
        {
            bestDistanceProgressValue = maxProgressValue;
            bestDistanceValueAtMax = true;
        }
        overlayCanvas.DrawBestDistanceBar(bestDistanceProgressValue);
    }

    void SetUpLevelStars()
    {
        float[] values = new float[3];

        for (int i = 0; i < starDistances.Length; i++)
        {
            values[i] = (float)starDistances[i] / (float)starDistances[2] * star3SliderValue;

            levelStars[i].SetUnlocked(gameManager.starUnlockStates[i]);
            levelStars[i].SetUnlockDistance(starDistances[i]);
        }

        overlayCanvas.SetLevelStarValues(values);


    }

    void Update()
    {
        UpdateProgressBar();
    }

    void UpdateProgressBar()
    {
        if (hasSurpassedStar3) return;

        float distance = scoring.trueDistance;
        int star3Distance = starDistances[2];
        float progressValue = distance / (float)star3Distance * star3SliderValue;

        overlayCanvas.DrawProgressBar(progressValue);

        if (distance > gameManager.highScore)
        {
            overlayCanvas.DrawBestDistanceBar(progressValue);
        }

        if (progressValue > star3SliderValue)
        {
            hasSurpassedStar3 = true;

            StartCoroutine(SmoothlyTransitionSliderToMax(progressValue));
        }

    }

    IEnumerator SmoothlyTransitionSliderToMax(float progressValue)
    {
        while (progressValue < maxProgressValue)
        {
            float delta = transitionToMaxValueSpeed * Time.deltaTime;
            progressValue = Mathf.MoveTowards(progressValue, maxProgressValue, delta);

            overlayCanvas.DrawProgressBar(progressValue);

            if (!bestDistanceValueAtMax)
            {
                overlayCanvas.DrawBestDistanceBar(progressValue);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    void CheckStarUnlock(int scoreDistance)
    {
        for (int i = 0; i < starDistances.Length; i++)
        {
            if (gameManager.starUnlockStates[i] == true) continue;

            if (scoreDistance >= starDistances[i])
            {
                gameManager.starUnlockStates[i] = true;
                UnlockStar(i);
            }
            else
            {
                return;
            }
        }
    }

    void UnlockStar(int starIndex)
    {
        levelStars[starIndex].SetUnlocked(true);
    }

    void OnEnable()
    {
        Scoring.OnUpdatedScore += CheckStarUnlock;
    }

    void OnDisable()
    {
        Scoring.OnUpdatedScore -= CheckStarUnlock;
    }
}
