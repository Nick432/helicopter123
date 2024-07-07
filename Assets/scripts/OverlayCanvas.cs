using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI distanceScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] Slider snowballSizeSlider;

    Game_Manager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<Game_Manager>();
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
        string textToDisplay = "Best: " + distance.ToString("n0") + " m";
        highScoreText.text = textToDisplay;
    }

    public void DrawSizeMeter(float sizePercentage)
    {
        snowballSizeSlider.value = sizePercentage;
    }
}
