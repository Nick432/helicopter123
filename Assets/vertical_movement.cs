using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vertical_movement : MonoBehaviour
{

    [HideInInspector] public Vector3 Direction = new Vector3(1, 0, 0);
    [SerializeField] float vertical_speed;

    [HideInInspector] public OutOfBoundsBox outOfBoundsBox;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Change_Direction());

        outOfBoundsBox = FindObjectOfType<OutOfBoundsBox>();
    }

    // Update is called once per frame
    void Update()
    {


        transform.position += (Direction * vertical_speed) * Time.deltaTime;



    }


    IEnumerator Change_Direction()
    {

        while (true)
        {

            Direction.x *= -1f;


            yield return new WaitForSeconds(3);

        }

    }




}
