using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class OverlayCanvas : MonoBehaviour
{
    [Header("Scoring & Size Meter")]
    [SerializeField] TextMeshProUGUI distanceScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] Slider snowballSizeSlider;
    [Header("Level Progress")]
    [SerializeField] Slider progressBar;
    [SerializeField] Slider bestDistanceBar;
    [SerializeField] Slider bestDistanceMarker;
    [SerializeField] Slider[] starSliders = new Slider[3];

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        DrawHighScore(gameManager.highScore);
    }

    public void DrawDistanceScore(int distance)
    {
        string textToDisplay = distance.ToString("n0") + " m";
        distanceScoreText.text = textToDisplay;
    }

    public void DrawHighScore(int distance)
    {
        string textToDisplay = distance.ToString("n0") + " m";
        highScoreText.text = textToDisplay;
    }

    public void DrawSizeMeter(float sizePercentage)
    {
        snowballSizeSlider.value = sizePercentage;
    }

    public void SetLevelStarValues(float[] values)
    {
        if (values.Length != starSliders.Length) return;

        for (int i = 0; i < starSliders.Length; i++)
        {
            starSliders[i].value = values[i];
        }
    }

    public void DrawProgressBar(float value)
    {
        progressBar.value = value;
    }

    public void DrawBestDistanceBar(float value)
    {
        bestDistanceBar.value = value;
        bestDistanceMarker.value = value;
    }
}
