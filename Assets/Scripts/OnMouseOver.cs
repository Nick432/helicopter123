using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<bool> OnMouseOverEvent;


    public void OnPointerEnter(PointerEventData pointerEventData) 
    {
        OnMouseOverEvent.Invoke(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        OnMouseOverEvent.Invoke(false);
    }   
}
