using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundManager : MonoBehaviour
{
    [Header("Changing Backgrounds")]
    [SerializeField] int distanceForThinSnow = 2000;
    [SerializeField] int distanceForDirtSnow = 4000;
    [SerializeField] float fadeSpeed = 1f;

    [Header("PassiveRateOfSize")]
    [SerializeField] float sizeRateWithDeepSnow;
    [SerializeField] float sizeRateWithThinSnow;
    [SerializeField] float sizeRateWithDirtSnow;

    [Header("References")]
    [SerializeField] public SpriteRenderer deepSnowBackgroundSprite;
    [SerializeField] public SpriteRenderer thinSnowBackgroundSprite;
    
    [SerializeField] SnowballSizeManager snowballSizeManager;

    public bool deepSnowVisible = true;
    public bool thinSnowVisible = true;

    void Start()
    {
        // Set passive rate to snowballSize script.
        snowballSizeManager.SetPassiveSizeRate(sizeRateWithDeepSnow);
    }

    // Script called when score changes. Used to change background and passiveSizeRate.
    public void HandleBackgroundChange(int score)
    {
        if (deepSnowVisible && score >= distanceForThinSnow)
        {
            StartCoroutine(FadeBackground(deepSnowBackgroundSprite));
            deepSnowVisible = false;
            snowballSizeManager.SetPassiveSizeRate(sizeRateWithThinSnow);
        }
        if (thinSnowVisible && score >= distanceForDirtSnow)
        {
            StartCoroutine(FadeBackground(thinSnowBackgroundSprite));
            thinSnowVisible = false;
            snowballSizeManager.SetPassiveSizeRate(sizeRateWithDirtSnow);
        }




    }

    IEnumerator FadeBackground(SpriteRenderer backgroundSprite)
    {
        // Shift colour until background has fully faded.
        float alpha = backgroundSprite.color.a;
        float  originalalpha = alpha;
        while (alpha > 0f)
        {
            alpha = Mathf.MoveTowards(alpha, 0f, fadeSpeed * Time.deltaTime);
            Color newColour = backgroundSprite.color;
            newColour.a = alpha;
            backgroundSprite.color = newColour;

            yield return new WaitForEndOfFrame();
        }
    }

    // Listening to event from scoring script.
    void OnEnable() 
    {
        Scoring.OnUpdatedScore += HandleBackgroundChange;
    }

    void OnDisable() 
    {
        Scoring.OnUpdatedScore -= HandleBackgroundChange;
    }
}
