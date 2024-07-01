using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{


    [SerializeField] float speed;
    [SerializeField] GameObject fuelmeter;


    [HideInInspector] public float fuel_level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {




        // update input here
        Vector3 Movevector = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {


          
            Movevector.x -= speed;


            //transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Movevector.x += speed;



        }
        if (Input.GetKey(KeyCode.S))
        {

            Movevector.y -= speed;


        }
        if (Input.GetKey(KeyCode.W))
        {
            Movevector.y += speed;


        }




        transform.position += Movevector;





    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.tag == "fuel")
        {

            Destroy(collision.gameObject);


            fuelmeter.GetComponent<fuel_meter>().fuelamount += 0.05f;

        }


    }

}
