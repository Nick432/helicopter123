using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Contactable : MonoBehaviour
{
    public UnityEvent<GameObject> OnContact;

    public void Contacted(GameObject otherObject)
    {
        OnContact.Invoke(otherObject);
    }
}
