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
    [SerializeField] List<float> sizeRates = new List<float>();

    [Header("References")]
    public SpriteRenderer deepSnowBackgroundSprite;
    public SpriteRenderer thinSnowBackgroundSprite;
    public SpriteRenderer dirtSnowBackgroundSprite;

    public static event Action<float> OnSetPassiveSizeRate;

    [HideInInspector] public bool deepSnowVisible = true;
    [HideInInspector] public bool thinSnowVisible = true;
    [HideInInspector] public bool dirtSnowVisible = true;

    [HideInInspector] public Coroutine fadeDeepSnowCoroutine;
    [HideInInspector] public Coroutine fadeThinSnowCoroutine;
    [HideInInspector] public Coroutine fadeDirtSnowCoroutine;

    public bool overrideBackgrounds;

    void Start()
    {
        // Set passive rate to snowballSize script.
        OnSetPassiveSizeRate?.Invoke(sizeRates[0]);
    }

    void OnUpdatedScore(int score)
    {
        if (deepSnowVisible && score >= distanceForThinSnow)
        {
            deepSnowVisible = false;
            if (overrideBackgrounds) return;
            SetBackgroundFade(fadeDeepSnowCoroutine, deepSnowBackgroundSprite, 0, true);
        }
        if (thinSnowVisible && score >= distanceForDirtSnow)
        {
            thinSnowVisible = false;
            if (overrideBackgrounds) return;
            SetBackgroundFade(fadeThinSnowCoroutine, thinSnowBackgroundSprite, 1, true);
        }
        if (dirtSnowVisible && score >= distanceForNoSnow)
        {
            dirtSnowVisible = false;
            if (overrideBackgrounds) return;
            SetBackgroundFade(fadeDirtSnowCoroutine, dirtSnowBackgroundSprite, 2, true);
        }
    }

    public void SetBackgroundFade(Coroutine coroutine, SpriteRenderer backgroundSpriteRenderer, 
                           int backgroundIndex, bool disappearing)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(FadeBackground(backgroundSpriteRenderer, disappearing));

        int sizeRateIndex = backgroundIndex;
        if (disappearing)
        {
            sizeRateIndex = backgroundIndex + 1;
        }
        OnSetPassiveSizeRate?.Invoke(sizeRates[sizeRateIndex]);
    }

    IEnumerator FadeBackground(SpriteRenderer backgroundSprite, bool disappearing)
    {
        // Shift colour until background has fully faded.
        float alpha = backgroundSprite.color.a;
        float target = 1f;
        if (disappearing)
        {
            target = 0f;
        }

        while (alpha != target)
        {
            alpha = Mathf.MoveTowards(alpha, target, fadeSpeed * Time.deltaTime);
            Color newColour = backgroundSprite.color;
            newColour.a = alpha;
            backgroundSprite.color = newColour;

            yield return new WaitForEndOfFrame();
        }
    }

    // Listening to event from scoring script.
    void OnEnable() 
    {
        Scoring.OnUpdatedScore += OnUpdatedScore;
    }

    void OnDisable() 
    {
        Scoring.OnUpdatedScore -= OnUpdatedScore;
    }
}
