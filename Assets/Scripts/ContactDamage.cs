using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [Header("Contact Damage")]
    public float contactAmount;
    [SerializeField] bool healInsteadOfDamage;

    [Header("Behaviour")]
    [SerializeField] float immunityTimeAfterContact = 1f;
    [SerializeField] bool destroyOnContact;

    [HideInInspector] public bool contactable = true;


    public void HandleContactBehaviour(GameObject otherObject)
    {
        if (!contactable) return;

        SnowballSizeManager snowballSizeManager = otherObject.GetComponent<SnowballSizeManager>();

        if (snowballSizeManager != null)
        {
            HandlePlayerContact(snowballSizeManager);
        }

        
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

    void HandlePlayerContact(SnowballSizeManager snowballSizeManager)
    {

        if (healInsteadOfDamage)
        {
            snowballSizeManager.OnContact(contactAmount, gameObject);
        }
        else
        {
            snowballSizeManager.OnContact(-contactAmount, gameObject);
        }
    }

    IEnumerator HandleImmunityTimer()
    {
        yield return new WaitForSeconds(immunityTimeAfterContact);

        contactable = true;
    }
}
