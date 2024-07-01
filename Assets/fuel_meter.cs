using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuel_meter : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector] public float fuelamount = 1.0f;

    public float scalePercentage = 1.0f;  

    private Vector3 originalScale;
    void Start()
    {
        originalScale = transform.localScale;

        
        transform.localScale = originalScale * scalePercentage;
    }

    // Update is called once per frame
    void Update()
    {
       fuelamount -= 0.0001f;

        if (scalePercentage > 0)
        {
            scalePercentage = fuelamount;
        }
        else
        {
            //Game end logic here

        }
      
        Vector3 ModifiedScale = new Vector3(originalScale.x, originalScale.y * scalePercentage,originalScale.z);

        transform.localScale = ModifiedScale;

    }
}
