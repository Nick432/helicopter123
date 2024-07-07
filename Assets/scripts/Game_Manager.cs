using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    [Header("Global Move Speed")]
    public float globalBaseMoveSpeedMax = 5f;
    [SerializeField] AnimationCurve moveSpeedBySizeCurve;
    [SerializeField] float moveSpeedIncreasePerWave;
    [SerializeField] float moveSpeedTransitionSpeed;

    [HideInInspector] public float globalBaseMoveSpeed;
    [HideInInspector] public float initialGlobalBaseMoveSpeed;

    [Header("Game Over")]
    [SerializeField] float reloadDelay;

    SnowballSizeManager snowballSizeManager;

    [HideInInspector] public int highScore;

    void Awake()
    {
        initialGlobalBaseMoveSpeed = globalBaseMoveSpeedMax;
    }

    void Start()
    {
        ManageSingleton();
    }

    public void HandleOnGameStart()
    {
        snowballSizeManager = FindObjectOfType<SnowballSizeManager>();
        globalBaseMoveSpeedMax = initialGlobalBaseMoveSpeed;
    }

    // Don't destroy the gameManager on load. This means certain variables are not reset when
    // the game resets.
    void ManageSingleton()
    {
        int instanceCount = FindObjectsOfType<Game_Manager>().Length;

        //If there is more than one GameManager, destroy the newest one.
        if (instanceCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        float sizePercentage = snowballSizeManager.GetSizePercentage();
        globalBaseMoveSpeed = globalBaseMoveSpeedMax * 
                                 moveSpeedBySizeCurve.Evaluate(sizePercentage);
    }

    void HandleGameOver()
    {
        globalBaseMoveSpeedMax = 0f;
        StartCoroutine(ReloadGame());
    }

    IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(reloadDelay);

        SceneManager.LoadScene(0);
    }

    public void IncreaseGlobalBaseMoveSpeed()
    {
        StartCoroutine(TransitionMoveSpeed());
    }

    IEnumerator TransitionMoveSpeed()
    {
        float currentMoveSpeed = globalBaseMoveSpeedMax;
        float targetMoveSpeed = globalBaseMoveSpeedMax + moveSpeedIncreasePerWave;
        while (currentMoveSpeed < targetMoveSpeed)
        {
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, targetMoveSpeed,
                                                 moveSpeedTransitionSpeed * Time.deltaTime);
            globalBaseMoveSpeedMax = currentMoveSpeed;

            yield return new WaitForEndOfFrame();
        }
        globalBaseMoveSpeedMax = targetMoveSpeed;
    }

    void OnEnable() 
    {
        SnowballSizeManager.OnGameOver += HandleGameOver;
    }

    void OnDisable()
    {
        SnowballSizeManager.OnGameOver -= HandleGameOver;
    }

}
