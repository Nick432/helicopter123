using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float gameOverScreenDelay;

     GameOverCanvas gameOverCanvas;

    // Stored values across runs.

    [HideInInspector] public int highScore;
    [HideInInspector] public bool[] starUnlockStates = new bool[3];
     public float maxSize;
     public float toughness;

    void Awake()
    {
        ManageSingleton();

        maxSize = 100f;
    }

    public void HandleOnGameStart()
    {
        gameOverCanvas = FindObjectOfType<GameOverCanvas>();
    }

    // Don't destroy the gameManager on load. This means certain variables are not reset when
    // the game resets.
    void ManageSingleton()
    {
        int instanceCount = FindObjectsOfType<GameManager>().Length;

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

    void HandleGameOver()
    {
        StartCoroutine(DisplayGameOverScreen());
    }

    IEnumerator DisplayGameOverScreen()
    {
        yield return new WaitForSeconds(gameOverScreenDelay);

        gameOverCanvas.DisplayCanvas(true);
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
