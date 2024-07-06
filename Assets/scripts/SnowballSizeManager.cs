using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class SnowballSizeManager : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 1f)] float initialSize;

    
    
    [SerializeField] OverlayCanvas overlayCanvas;

    public GameObject GameManager;
    public GameObject BackgroundManager;  

    float sizePercentage;
    float currentSizeRate;
    float passiveSizeRate;

    bool gameOver;

    void Start()
    {
        sizePercentage = initialSize;
        UpdateCurrentSizeRate();
    }

    void UpdateCurrentSizeRate()
    {
        currentSizeRate = passiveSizeRate;
    }

    void Update()
    {
        if (gameOver)
        {


            var scoring = GameManager.GetComponent<Scoring>();
            var Bm = BackgroundManager.GetComponent<BackgroundManager>();



            if (scoring.scoreDistance > scoring.BestScoreDistance) {
                scoring.BestScoreDistance = scoring.scoreDistance;
                overlayCanvas.DrawBestDistanceScore(scoring.BestScoreDistance);

                    }


            Bm.deepSnowVisible = true;

            Bm.thinSnowVisible = true;
            Color originalAlpha1 = Bm.deepSnowBackgroundSprite.color;
            originalAlpha1.a = 1;


            Color originalAlpha2 = Bm.thinSnowBackgroundSprite.color;
            originalAlpha2.a = 1;


            Bm.deepSnowBackgroundSprite.color = originalAlpha1;

            Bm.thinSnowBackgroundSprite.color = originalAlpha2;

            scoring.gameTime = 0;
            scoring.scoreDistance = 0;
            


            sizePercentage = initialSize;

            
           //  
            //  SetPassiveSizeRate(sizeRateWithDeepSnow);
              SetPassiveSizeRate(0.2f);
            Bm.HandleBackgroundChange(scoring.scoreDistance);



           

            // scoring.trueDistance = 0;
            //  sizePercentage = initialSize;
            //  UpdateCurrentSizeRate();
            gameOver = false;
           

        }




        HandleSizeChange();
        DrawSizeToUI();
    }

    void HandleSizeChange()
    {
        if (sizePercentage > 0f)
        {
            sizePercentage += currentSizeRate * Time.deltaTime;
            sizePercentage = Mathf.Clamp(sizePercentage, 0f, 1f);
        }
        else
        {
            sizePercentage = 0f;
            gameOver = true;

            Debug.Log("Out of snow!!!");

           // ResetGame();





            //reset score, set high score
            //reset background manager
            //reset snowball
            // Game over logic
        }
    }
    
    void DrawSizeToUI()
    {
        overlayCanvas.DrawFuelGauge(sizePercentage);
    }

    public void AddSizePercentage(float value)
    {
        sizePercentage += value;
        sizePercentage = Mathf.Clamp(sizePercentage, 0f, 1f);
    }

    // Used by background manager to change the passiveSizeRate.
    public void SetPassiveSizeRate(float rate)
    {
        passiveSizeRate = rate;
        UpdateCurrentSizeRate();
    }

    public float GetSizePercentage()
    {
        return sizePercentage;
    }
}
