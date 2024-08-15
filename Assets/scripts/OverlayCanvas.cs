using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public class OverlayCanvas : MonoBehaviour
{
    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI distanceScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [Header("Size Meter")]
    [SerializeField] Slider mainSizeSlider;
    [SerializeField] Slider reducedSizeSlider;
    [SerializeField] Slider increasedSizeSlider;
    [SerializeField] RectTransform sizeMeterLayoutGroupRect;
    [Header("Level Progress")]
    [SerializeField] Slider progressBar;
    [SerializeField] Slider bestDistanceBar;
    [SerializeField] Slider bestDistanceMarker;
    [SerializeField] Slider[] starSliders = new Slider[3];

    GameManager gameManager;
    LayoutElement mainSizeSliderLayoutElement;

    float sizeMeterWidth;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        mainSizeSliderLayoutElement = mainSizeSlider.GetComponent<LayoutElement>();
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

    public void DrawSizeMeter(float sizeDifferencePercentage)
    {
        float fullWidth = sizeMeterLayoutGroupRect.rect.width;

        if (sizeDifferencePercentage == 1f)
        {
            reducedSizeSlider.gameObject.SetActive(false);
            increasedSizeSlider.gameObject.SetActive(false);
        }
        else if (sizeDifferencePercentage < 1f)
        {
            reducedSizeSlider.gameObject.SetActive(true);
            increasedSizeSlider.gameObject.SetActive(false);

            mainSizeSliderLayoutElement.minWidth = fullWidth * sizeDifferencePercentage;
        }
        else
        {
            reducedSizeSlider.gameObject.SetActive(false);
            increasedSizeSlider.gameObject.SetActive(true);

            float widthPercentage = 1f / sizeDifferencePercentage;
            mainSizeSliderLayoutElement.minWidth = fullWidth * widthPercentage;
        }

    }

    public void DrawSnowballSize(float sizePercentage, float sizeDifferencePercentage)
    {
        if (sizeDifferencePercentage > 1f)
        {
            mainSizeSlider.value = sizePercentage * sizeDifferencePercentage;
            if (increasedSizeSlider.enabled == true)
            {
                float percentage = (sizePercentage * sizeDifferencePercentage - 1f) /
                                   (sizeDifferencePercentage - 1f);
                increasedSizeSlider.value = percentage;
            }
        }
        else
        {
            mainSizeSlider.value = sizePercentage;
        }
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
