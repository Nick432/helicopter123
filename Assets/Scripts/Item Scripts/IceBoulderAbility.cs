using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoulderAbility : MonoBehaviour
{
    [SerializeField] Sprite abilityIcon;
    public List<float> durations = new List<float>();
    public List<float> coolDowns = new List<float>();
    public List<float> sizeIncreases = new List<float>();
    

    ItemScriptManager itemScriptManager;
    SnowballSizeManager snowballSizeManager;
    AbilityManager abilityManager;
    Ability ability;

    int upgradeLevel;
    int abilityIndex;

    float originalSize;
    float originalMaxSize;
    float originalDamageReduction;
    
    void Awake()
    {
        itemScriptManager = GetComponent<ItemScriptManager>();
    }

    void OnGameStart()
    {
        snowballSizeManager = FindObjectOfType<SnowballSizeManager>();
        abilityManager = FindObjectOfType<AbilityManager>();
        if (snowballSizeManager == null) return;
        if (abilityManager == null) return;

        upgradeLevel = itemScriptManager.upgradeLevel;
        abilityIndex = itemScriptManager.abilityIndex;

        ability = abilityManager.GetAbility(abilityIndex);
        ability.StartUpAbility(itemScriptManager, abilityIcon, coolDowns[upgradeLevel - 1], 
                               durations[upgradeLevel - 1]);
    }

    public void ActivateAbility()
    {
        originalMaxSize = snowballSizeManager.maxSize;
        originalSize = snowballSizeManager.size;
        originalDamageReduction = snowballSizeManager.GetDamageReduction();

        float newMaxSize = originalMaxSize + sizeIncreases[upgradeLevel - 1];
        newMaxSize = Mathf.Clamp(newMaxSize, 0f, 300f);

        snowballSizeManager.SetAllMaxSize(newMaxSize);
        snowballSizeManager.size = snowballSizeManager.maxSize;
        snowballSizeManager.SetDamageReduction(1f);

        StartCoroutine(IceBoulder());
    }

    IEnumerator IceBoulder()
    {
        yield return new WaitForSeconds(durations[upgradeLevel - 1]);

        snowballSizeManager.size = originalSize;
        snowballSizeManager.SetAllMaxSize(originalMaxSize);
        snowballSizeManager.SetDamageReduction(originalDamageReduction);
        bool alreadyImmune = snowballSizeManager.immune;
        snowballSizeManager.immune = true;

        yield return new WaitForSeconds(0.5f);
        if (!alreadyImmune)
        {
            snowballSizeManager.immune = false;
        }
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
