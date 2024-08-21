using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopPlayerInput : MonoBehaviour
{
    public static event Action OnClickEvent;
    public static event Action<bool> OnScrollEvent;

    void OnClick()
    {
        OnClickEvent?.Invoke();
    }

    void OnScroll(InputValue inputValue)
    {
        float rawInput = inputValue.Get<Vector2>().y;

        if (rawInput == 0f) return;
        bool scrollingUp = rawInput > 0f;

        OnScrollEvent?.Invoke(scrollingUp);        
    }
}
