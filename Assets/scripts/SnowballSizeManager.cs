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

    OverlayCanvas overlayCanvas;
    AudioSource audioSource;

    float size;
    [HideInInspector] public float sizeDifferenceFromDefaultPercentage;

     public float maxSize = 100f;

    float currentSizeRate;
    float passiveSizeRate;
    float overrideSizeRate;
    bool useOverrideSizeRate;

    List<ApplySizeRate> applySizeRates = new List<ApplySizeRate>();

    public static event Action OnGameOver;

    bool gameOver;

    void Awake()
    {
        overlayCanvas = FindObjectOfType<OverlayCanvas>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        size = initialSize;

        sizeDifferenceFromDefaultPercentage = maxSize / defaultMaxSize;

        overlayCanvas.DrawSizeMeter(sizeDifferenceFromDefaultPercentage);
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
            size += currentSizeRate * Time.deltaTime;
            size = Mathf.Clamp(size, 0f, maxSize);
        }
        else
        {
            size = 0f;
            gameOver = true;

            OnGameOver?.Invoke();

            Instantiate(hitEffect, transform.position, quaternion.identity);
        }
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
        size += contactAmount;
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
        }
        else
        {
            // Prioritise using the largest rate if multiple are set simultaneously.
            // Order list by largest sizeRateOnContact to smallest.
            applySizeRates = applySizeRates.OrderByDescending(x => x.sizeRateOnContact).ToList();
            // Use the largest rate.
            overrideSizeRate = applySizeRates[0].sizeRateOnContact;

            useOverrideSizeRate = true;
        }
        
        UpdateCurrentSizeRate();
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
