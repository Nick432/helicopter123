using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{

    [HideInInspector] public GameObject[] items; 

    [HideInInspector] public  GameObject[] Obstacles;

    [HideInInspector] public float fuel_level;

    [SerializeField] GameObject fuel;

    [SerializeField] GameObject banana;

    [SerializeField] int spawn_amount;

    public int elapsed_time = 0;

    
    


    // Start is called before the first frame update
    void Start()
    {
     Obstacles    = GameObject.FindGameObjectsWithTag("obstacle");

   //  items        = GameObject.FindGameObjectsWithTag("item");
    }

    // Update is called once per frame
    void Update()
    {

        // spawn  new fuel pickups every 5 seconds

        if (wait(5000))
        {

            spawnfuel();
            //now that fuel has been update the item_array
           // items = GameObject.FindGameObjectsWithTag("item");
        }

        //update fuel bar each tick


    }


    void spawnfuel()
    {

        Camera camera = Camera.main;
        // seperate spawn y cooderinates to  just above the camera

        //randomise spawn x value on the screen 

        for (int i = 0; i < spawn_amount; i++)
        {

            float rx = Random.Range(0f  - (camera.pixelWidth / 100) , (camera.pixelWidth / 100));

            float ydisplacement = Random.Range(0f, 3f);

            float y = 0 + (camera.pixelHeight / 100) + ydisplacement  ;

            Vector3 spawnpos = new Vector3(rx, y, 0f);

            Instantiate(fuel, spawnpos, Quaternion.identity);

        }

    }

    //update fuel meter gui and fuel
   

  



    //every frame update time by 1 if it is less then day otherwise reset time and return true
    bool  wait(int Delay)
    {

        if(elapsed_time < Delay)
        {

            elapsed_time += 1;
            return false;
        }

        else
        {

            elapsed_time = 0;
            return true;
        }


    }
}
