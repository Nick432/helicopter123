using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteOutAbility : MonoBehaviour
{
    [SerializeField] Sprite abilityIcon;
    public List<float> durations = new List<float>();
    public List<float> coolDowns = new List<float>();
    public List<float> snowingRates = new List<float>();
    

    ItemScriptManager itemScriptManager;
    BackgroundManager backgroundManager;
    SnowballSizeManager snowballSizeManager;
    AbilityManager abilityManager;
    SnowingParticles snowingParticles;
    Ability ability;

    int upgradeLevel;
    int abilityIndex;
    
    void Awake()
    {
        itemScriptManager = GetComponent<ItemScriptManager>();
    }

    void OnGameStart()
    {
        snowingParticles = FindObjectOfType<SnowingParticles>();
        backgroundManager = FindObjectOfType<BackgroundManager>();
        abilityManager = FindObjectOfType<AbilityManager>();
        snowballSizeManager = FindObjectOfType<SnowballSizeManager>();
        if (snowingParticles == null) return;
        if (backgroundManager == null) return;
        if (abilityManager == null) return;
        if (snowballSizeManager == null) return;

        upgradeLevel = itemScriptManager.upgradeLevel;
        abilityIndex = itemScriptManager.abilityIndex;

        ability = abilityManager.GetAbility(abilityIndex);
        ability.StartUpAbility(itemScriptManager, abilityIcon, coolDowns[upgradeLevel - 1], 
                               durations[upgradeLevel - 1]);
    }

    public void ActivateAbility()
    {
        StartCoroutine(WhiteOut());
    }

    IEnumerator WhiteOut()
    {
        float quarterDuration = durations[upgradeLevel - 1] / 4f;
        backgroundManager.overrideBackgrounds = true;
        snowballSizeManager.isSnowing = true;
        snowballSizeManager.snowingRate = snowingRates[upgradeLevel - 1];
        snowingParticles.SetSnowing(true);

        yield return new WaitForSeconds(quarterDuration);

        backgroundManager.SetBackgroundFade(backgroundManager.fadeDirtSnowCoroutine,
                                            backgroundManager.dirtSnowBackgroundSprite,
                                            2, false);

        yield return new WaitForSeconds(quarterDuration);

        backgroundManager.SetBackgroundFade(backgroundManager.fadeThinSnowCoroutine,
                                            backgroundManager.thinSnowBackgroundSprite,
                                            1, false);

        yield return new WaitForSeconds(quarterDuration);

        backgroundManager.SetBackgroundFade(backgroundManager.fadeDeepSnowCoroutine,
                                            backgroundManager.deepSnowBackgroundSprite,
                                            0, false);

        yield return new WaitForSeconds(quarterDuration);

        snowballSizeManager.isSnowing = false;
        snowingParticles.SetSnowing(false);

        yield return new WaitForSeconds(quarterDuration * 2f);

        backgroundManager.SetBackgroundFade(backgroundManager.fadeDeepSnowCoroutine,
                                            backgroundManager.deepSnowBackgroundSprite,
                                            0, true);

        yield return new WaitForSeconds(1f);

        backgroundManager.SetBackgroundFade(backgroundManager.fadeThinSnowCoroutine,
                                            backgroundManager.thinSnowBackgroundSprite,
                                            1, true);

        yield return new WaitForSeconds(1f);

        backgroundManager.SetBackgroundFade(backgroundManager.fadeDirtSnowCoroutine,
                                            backgroundManager.dirtSnowBackgroundSprite,
                                            2, true);

        backgroundManager.overrideBackgrounds = false;
    }

    void OnEnable()
    {
        Snowball.OnGameStart += OnGameStart;
    }

    void OnDisable()
    {
        Snowball.OnGameStart -= OnGameStart;
    }
}
