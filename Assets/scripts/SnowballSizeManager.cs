using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Unity.Mathematics;

public class SnowballSizeManager : MonoBehaviour
{
    [Header("Size")]
    [SerializeField] float initialSize = 1f;
    public float defaultMaxSize = 100f;

    [Header("Effects")]
    [SerializeField] GameObject hitEffect;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip snowHitSound;

    [Header("Resurrection")]
    [SerializeField] Color immunityColour;
    [SerializeField] GameObject scaleAnchor;

    [Header("Damage Reduction")]
    [SerializeField] SpriteRenderer defaultSpriteRenderer;
    [SerializeField] SpriteRenderer icySpriteRenderer;

    SpriteRenderer[] spriteRenderers;


    OverlayCanvas overlayCanvas;
    AudioSource audioSource;
    Scoring scoring;

    [HideInInspector] public float size;
    [HideInInspector] public float sizeDifferenceFromDefaultPercentage;

    public float maxSize;

    float currentSizeRate;
    float passiveSizeRate;
    float overrideSizeRate;
    bool useOverrideSizeRate;

    List<ApplySizeRate> applySizeRates = new List<ApplySizeRate>();

    public static event Action OnGameOver;
    public static event Action<float> OnUpdateAllMaxSize;

    GameManager gameManager;

    bool gameOver;
    float damageReduction;

    // Bonus Stats
    [HideInInspector] public float dirtReduction;
    [HideInInspector] public float treeReduction;
    [HideInInspector] public float chanceForTreeCash;
    [HideInInspector] public int treeCashAmount;
    [HideInInspector] public float allSnowIncrease;
    [HideInInspector] public float snowPatchAndPileReduction;
    [HideInInspector] public bool earnMoneyFromSnowPatch;
    [HideInInspector] public float earnDelay = 2f;
    Coroutine earnMoneyFromSnowPatchCoroutine;
    [HideInInspector] public bool resurrectOnDeath;
    [HideInInspector] public float resurrectSize;
    [HideInInspector] public float resurrectImmunityTime = 1f;
    float resurrectDelay = 1f;
    [HideInInspector] public bool immune;
    [HideInInspector] public bool isSnowing;
    [HideInInspector] public float snowingRate;

    void Awake()
    {
        overlayCanvas = FindObjectOfType<OverlayCanvas>();
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        scoring = FindObjectOfType<Scoring>();
        maxSize = gameManager.maxSize;
    }

    void Start()
    {
        size = initialSize;
        spriteRenderers = scaleAnchor.GetComponentsInChildren<SpriteRenderer>();
        
        sizeDifferenceFromDefaultPercentage = maxSize / defaultMaxSize;
        SetDamageReduction(gameManager.damageReduction);

        overlayCanvas.DrawSizeMeter(sizeDifferenceFromDefaultPercentage);
    }

    public void SetAllMaxSize(float value)
    {
        maxSize = value;
        sizeDifferenceFromDefaultPercentage = maxSize / defaultMaxSize;
        OnUpdateAllMaxSize?.Invoke(maxSize);
    }

    public void SetDamageReduction(float value)
    {
        damageReduction = value;
        if (damageReduction > 0f)
        {
            Color newColour = icySpriteRenderer.color;
            newColour.a = damageReduction;
            icySpriteRenderer.color = newColour;
        }
        else
        {
            Color newColour = icySpriteRenderer.color;
            newColour.a = 0f;
            icySpriteRenderer.color = newColour;
        }
        if (damageReduction < 0f)
        {
            Color newColour = defaultSpriteRenderer.color;
            newColour.a = 1f + damageReduction;
            defaultSpriteRenderer.color = newColour;
        }
        else
        {
            Color newColour = defaultSpriteRenderer.color;
            newColour.a = 1f;
            defaultSpriteRenderer.color = newColour;
        }
    }

    public float GetDamageReduction()
    {
        return damageReduction;
    }

    void UpdateCurrentSizeRate()
    {
        if (useOverrideSizeRate)
        {
            currentSizeRate = overrideSizeRate;
        }
        else
        {
            currentSizeRate = passiveSizeRate;
        }
    }

    void Update()
    {
        if (gameOver) return;
        
        HandleSizeChange();
        DrawSizeToUI();
    }

    void HandleSizeChange()
    {
        if (size > 0f)
        {
            float adjustedSizeRate = currentSizeRate;

            if (isSnowing)
            {
                adjustedSizeRate += snowingRate;
            }

            adjustedSizeRate *= 1f - damageReduction / 2f;

            if (adjustedSizeRate >= 0f)
            {
                adjustedSizeRate *= 1f + allSnowIncrease;
                if (useOverrideSizeRate)
                {
                    adjustedSizeRate *= 1f - snowPatchAndPileReduction;
                }
            }
            else
            {
                adjustedSizeRate *= 1f - dirtReduction;
            }

            if (immune)
            {
                if (adjustedSizeRate < 0f)
                {
                    adjustedSizeRate = 0f;
                }
            }
            
            size += adjustedSizeRate * Time.deltaTime;
            size = Mathf.Clamp(size, 0f, maxSize);
        }
        else
        {
            size = 0f;
            gameOver = true;

            if (resurrectOnDeath)
            {
                StartCoroutine(Resurrect());
                return;
            }


            OnGameOver?.Invoke();

            Instantiate(hitEffect, transform.position, quaternion.identity);
        }
    }

    IEnumerator Resurrect()
    {
        Color[] originalColours = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColours[i] = spriteRenderers[i].color;
        }

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        scaleAnchor.SetActive(false);

        yield return new WaitForSeconds(resurrectDelay);

        scaleAnchor.SetActive(true);
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            Color newColour = immunityColour;
            newColour.a = originalColours[i].a;
            spriteRenderers[i].color = newColour;
        }

        size = maxSize * resurrectSize;
        resurrectOnDeath = false;
        gameOver = false;
        Instantiate(hitEffect, transform.position, quaternion.identity);
        immune = true;

        yield return new WaitForSeconds(resurrectImmunityTime);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = originalColours[i];
        }

        float bufferTime = 0.5f;
        yield return new WaitForSeconds(bufferTime);

        immune = false;
    }
    
    void DrawSizeToUI()
    {
        float sizePercentage = size / maxSize;
        overlayCanvas.DrawSnowballSize(sizePercentage, sizeDifferenceFromDefaultPercentage);
    }

    // Used by background manager to change the passiveSizeRate.
    void SetPassiveSizeRate(float rate)
    {
        passiveSizeRate = rate;
        UpdateCurrentSizeRate();
    }

    public void OnContact(float contactAmount, GameObject otherObject)
    {
        float adjustedContactAmount = contactAmount * (1f - damageReduction);

        if (otherObject.tag == "Tree")
        {
            adjustedContactAmount *= 1f - treeReduction;
            bool rewardTreeCash = UnityEngine.Random.value <= chanceForTreeCash;

            if (rewardTreeCash)
            {
                scoring.AddCoins(treeCashAmount);
            }
        }
        if (adjustedContactAmount >= 0f)
        {
            adjustedContactAmount *= 1f + allSnowIncrease;
            adjustedContactAmount *= 1f - snowPatchAndPileReduction;
        }

        if (immune)
        {
            if (adjustedContactAmount < 0f)
            {
                return;
            }
        }

        size += adjustedContactAmount;
        size = Mathf.Clamp(size, 0f, maxSize);

        if (contactAmount > 0f)
        {
            audioSource.PlayOneShot(snowHitSound);
            Instantiate(hitEffect, otherObject.transform.position, quaternion.identity);
        }
        else
        {
            audioSource.PlayOneShot(crashSound);
            Instantiate(hitEffect, transform.position, quaternion.identity);
        }
    }

    public float GetSizePercentage()
    {
        return size / maxSize;
    }

    public float GetCurrentSizeRate()
    {
        return currentSizeRate;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        ApplySizeRate applySizeRate = other.GetComponentInParent<ApplySizeRate>();

        if (applySizeRate != null)
        {
            applySizeRates.Add(applySizeRate);
            HandleApplySizeRates();
        }

        Contactable contactable = other.GetComponentInParent<Contactable>();
        
        if (contactable != null)
        {
            contactable.Contacted(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        ApplySizeRate applySizeRate = other.GetComponentInParent<ApplySizeRate>();

        if (applySizeRate != null)
        {
            applySizeRates.Remove(applySizeRate);
            HandleApplySizeRates();
        }
    }

    void HandleApplySizeRates()
    {
        if (applySizeRates.Count == 0)
        {
            useOverrideSizeRate = false;

            if (earnMoneyFromSnowPatchCoroutine != null)
            {
                StopCoroutine(earnMoneyFromSnowPatchCoroutine);
                earnMoneyFromSnowPatchCoroutine = null;
            }
        }
        else
        {
            // Prioritise using the largest rate if multiple are set simultaneously.
            // Order list by largest sizeRateOnContact to smallest.
            applySizeRates = applySizeRates.OrderByDescending(x => x.sizeRateOnContact).ToList();
            // Use the largest rate.
            overrideSizeRate = applySizeRates[0].sizeRateOnContact;

            useOverrideSizeRate = true;

            if (earnMoneyFromSnowPatch)
            {
                if (overrideSizeRate > 0f)
                {
                    if (earnMoneyFromSnowPatchCoroutine == null)
                    {
                        earnMoneyFromSnowPatchCoroutine = StartCoroutine(EarnMoneyFromSnowPatch());
                    }
                }
            }
        }
        
        UpdateCurrentSizeRate();
    }

    IEnumerator EarnMoneyFromSnowPatch()
    {
        while (true)
        {
            scoring.AddCoins(1);
            yield return new WaitForSeconds(earnDelay);
        }
    }

    void OnEnable() 
    {
        BackgroundManager.OnSetPassiveSizeRate += SetPassiveSizeRate;
    }

    void OnDisable()
    {
        BackgroundManager.OnSetPassiveSizeRate -= SetPassiveSizeRate;
    }
}
