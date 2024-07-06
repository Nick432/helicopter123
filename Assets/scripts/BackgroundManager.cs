using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackgroundManager : MonoBehaviour
{
    [Header("Changing Backgrounds")]
    [SerializeField] int distanceForThinSnow = 2000;
    [SerializeField] int distanceForDirtSnow = 4000;
    [SerializeField] int distanceForNoSnow = 6000;
    [SerializeField] float fadeSpeed = 1f;

    [Header("PassiveRateOfSize")]
    [SerializeField] float sizeRateWithDeepSnow;
    [SerializeField] float sizeRateWithThinSnow;
    [SerializeField] float sizeRateWithDirtSnow;
    [SerializeField] float sizeRateWithNoSnow;

    [Header("References")]
    [SerializeField] SpriteRenderer deepSnowBackgroundSprite;
    [SerializeField] SpriteRenderer thinSnowBackgroundSprite;
    [SerializeField] SpriteRenderer dirtSnowBackgroundSprite;

    public static event Action<float> OnSetPassiveSizeRate;

    bool deepSnowVisible = true;
    bool thinSnowVisible = true;
    bool dirtSnowVisible = true;

    void Start()
    {
        // Set passive rate to snowballSize script.
        OnSetPassiveSizeRate?.Invoke(sizeRateWithDeepSnow);
    }

    // Script called when score changes. Used to change background and passiveSizeRate.
    void HandleBackgroundChange(int score)
    {
        if (deepSnowVisible && score >= distanceForThinSnow)
        {
            StartCoroutine(FadeBackground(deepSnowBackgroundSprite));
            deepSnowVisible = false;
            OnSetPassiveSizeRate?.Invoke(sizeRateWithThinSnow);
        }
        if (thinSnowVisible && score >= distanceForDirtSnow)
        {
            StartCoroutine(FadeBackground(thinSnowBackgroundSprite));
            thinSnowVisible = false;
            OnSetPassiveSizeRate?.Invoke(sizeRateWithDirtSnow);
        }
        if (dirtSnowVisible && score >= distanceForNoSnow)
        {
            StartCoroutine(FadeBackground(dirtSnowBackgroundSprite));
            dirtSnowVisible = false;
            OnSetPassiveSizeRate?.Invoke(sizeRateWithNoSnow);
        }
    }

    IEnumerator FadeBackground(SpriteRenderer backgroundSprite)
    {
        // Shift colour until background has fully faded.
        float alpha = backgroundSprite.color.a;

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
