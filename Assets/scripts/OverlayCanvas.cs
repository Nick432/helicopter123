using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayCanvas : MonoBehaviour
{
    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI distanceScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI collectedCoinsText;
    [Header("Size Meter")]
    [SerializeField] Slider mainSizeSlider;
    [SerializeField] Slider reducedSizeSlider;
    [SerializeField] Slider increasedSizeSlider1;
    [SerializeField] Slider increasedSizeSlider2;
    [SerializeField] RectTransform sizeMeterLayoutGroupRect;
    [Header("Level Progress")]
    [SerializeField] Slider progressBar;
    [SerializeField] Slider bestDistanceBar;
    [SerializeField] Slider bestDistanceMarker;
    [SerializeField] Slider[] starSliders = new Slider[3];
    [Header("Build Debugging")]
    [SerializeField] TextMeshProUGUI debugText;

    GameManager gameManager;
    LayoutElement mainSizeSliderLayoutElement;
    LayoutElement increaseSizeSlider1LayoutElement;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        mainSizeSliderLayoutElement = mainSizeSlider.GetComponent<LayoutElement>();
        increaseSizeSlider1LayoutElement = increasedSizeSlider1.GetComponent<LayoutElement>();
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
            increasedSizeSlider1.gameObject.SetActive(false);
            increasedSizeSlider2.gameObject.SetActive(false);
        }
        else if (sizeDifferencePercentage < 1f)
        {
            reducedSizeSlider.gameObject.SetActive(true);
            increasedSizeSlider1.gameObject.SetActive(false);
            increasedSizeSlider2.gameObject.SetActive(false);

            mainSizeSliderLayoutElement.minWidth = fullWidth * sizeDifferencePercentage;
        }
        else if (sizeDifferencePercentage > 2f)
        {
            reducedSizeSlider.gameObject.SetActive(false);
            increasedSizeSlider1.gameObject.SetActive(true);
            increasedSizeSlider2.gameObject.SetActive(true);

            mainSizeSliderLayoutElement.minWidth = fullWidth * 1f / sizeDifferencePercentage;
            increaseSizeSlider1LayoutElement.minWidth = fullWidth * 1f / sizeDifferencePercentage;
        }
        else
        {
            reducedSizeSlider.gameObject.SetActive(false);
            increasedSizeSlider1.gameObject.SetActive(true);
            increasedSizeSlider2.gameObject.SetActive(false);

            mainSizeSliderLayoutElement.minWidth = fullWidth * 1f / sizeDifferencePercentage;
            increaseSizeSlider1LayoutElement.minWidth = 0f;
        }

    }

    public void DrawSnowballSize(float sizePercentage, float sizeDifferencePercentage)
    {
        if (sizeDifferencePercentage > 1f)
        {
            float barsFilled = sizePercentage * sizeDifferencePercentage;
            mainSizeSlider.value = barsFilled;
            float length1 = sizeDifferencePercentage - 1f;
            length1 = Mathf.Clamp(length1, 0.01f, 1f);
            increasedSizeSlider1.value = (barsFilled - 1f) / length1;
            float length2 = sizeDifferencePercentage - 2f;
            length2 = Mathf.Clamp(length2, 0.01f, 1f);
            increasedSizeSlider2.value = (barsFilled - 2f) / length2;
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

    public void DrawCollectedCoins(int amount)
    {
        collectedCoinsText.text = amount.ToString();
    }

    public void DrawDebugInfo(string text)
    {
        debugText.text = text;
    }

    void OnUpdateAllMaxSize(float value)
    {
        DrawSizeMeter(FindObjectOfType<SnowballSizeManager>().sizeDifferenceFromDefaultPercentage);
    }

    void OnEnable()
    {
        SnowballSizeManager.OnUpdateAllMaxSize += OnUpdateAllMaxSize;
    }

    void OnDisable()
    {
        SnowballSizeManager.OnUpdateAllMaxSize -= OnUpdateAllMaxSize;
    }
}
