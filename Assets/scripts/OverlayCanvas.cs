using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI distanceScoreText;
    [SerializeField] TextMeshProUGUI BestdistanceScoreText;
    [SerializeField] Slider snowballSizeSlider;

    public void DrawDistanceScore(int distance)
    {
        string textToDisplay = distance.ToString("n0") + " m";
        distanceScoreText.text = textToDisplay;
    }


    public void DrawBestDistanceScore(int distance)
    {
        string textToDisplay = distance.ToString("n0") + " m";
        BestdistanceScoreText.text = textToDisplay;
    }

    public void DrawFuelGauge(float tankPercentage)
    {
        snowballSizeSlider.value = tankPercentage;
    }
}
