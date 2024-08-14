using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<bool> OnMouseOverEvent;
    public UnityEvent<bool, GameObject> OnMouseOverObjectReference;
    [SerializeField] GameObject objectReference;


    public void OnPointerEnter(PointerEventData pointerEventData) 
    {
        OnMouseOverEvent.Invoke(true);
        OnMouseOverObjectReference.Invoke(true, objectReference);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        OnMouseOverEvent.Invoke(false);
        OnMouseOverObjectReference.Invoke(false, objectReference);
    }   
}
