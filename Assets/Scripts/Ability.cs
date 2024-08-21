using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [SerializeField] Color onCoolDownColour;

    Image image;
    ItemScriptManager abilityManager;
    [HideInInspector] public float coolDownResetTime = 120;

    float coolDownTimer;
    float coolDownPercentage;
    float abilityDuration;

    bool abilityIsSet;
    bool abilityIsReady;
    bool abilityIsComplete = true;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void StartUpAbility(ItemScriptManager itemScriptManager, Sprite sprite, 
                               float coolDownTime, float duration)
    {
        abilityManager = itemScriptManager;
        image.sprite = sprite;
        coolDownResetTime = coolDownTime;
        abilityDuration = duration;
        abilityIsSet = true;

        StartCoolDownTimer();
    }

    void StartCoolDownTimer()
    {
        coolDownTimer = coolDownResetTime;
        image.color = onCoolDownColour;
        abilityIsReady = false;
    }

    void Update()
    {
        if (!abilityIsSet) return;
        if (abilityIsReady) return;

        coolDownTimer -= Time.deltaTime;
        coolDownPercentage = coolDownTimer / coolDownResetTime;
        image.fillAmount = 1f - coolDownPercentage;

        if (coolDownTimer <= 0f)
        {
            coolDownTimer = 0f;
            coolDownPercentage = 1f;
            image.fillAmount = 1f;
            image.color = Color.white;

            abilityIsReady = true;
        }
    }

    public void HandleInput()
    {
        if (!abilityIsReady) return;
        if (!abilityIsComplete) return;

        abilityManager.OnActivateAbility.Invoke();
        
        abilityIsComplete = false;

        StartCoroutine(WaitOutAbilityDuration());

    }

    IEnumerator WaitOutAbilityDuration()
    {
        float timer = abilityDuration;
        while(timer > 0f)
        {
            timer -= Time.deltaTime;
            image.fillAmount = timer / abilityDuration;

            yield return new WaitForEndOfFrame();
        }

        abilityIsComplete = true;
        StartCoolDownTimer();
    }

}
