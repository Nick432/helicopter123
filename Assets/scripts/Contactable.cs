using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contactable : MonoBehaviour
{
    [Header("Contact Damage")]
    [Range(0f, 1f)] public float contactDamage;
    [SerializeField] bool healInsteadOfDamage;

    [Header("Behaviour")]
    [SerializeField] float immunityTimeAfterContact = 1f;
    [SerializeField] bool destroyOnContact;



    public AudioClip crash;
    public AudioClip sucess;
    private AudioSource audioSource;




    [HideInInspector] public bool contactable = true;

    void Start()
    {
       
        if (!healInsteadOfDamage)
        {
            contactDamage *= -1f;
            // play crash sound
          
        }


      
        // play heal sound
    }

    public void HandleContactBehaviour()
    {
        
        if (destroyOnContact)
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            contactable = false;

            StartCoroutine(HandleImmunityTimer());
        }
    }

    IEnumerator HandleImmunityTimer()
    {
        yield return new WaitForSeconds(immunityTimeAfterContact);

        contactable = true;
    }
}
