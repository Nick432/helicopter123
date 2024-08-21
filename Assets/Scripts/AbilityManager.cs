using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] Ability ability1;
    [SerializeField] Ability ability2;

    public Ability GetAbility(int index)
    {
        if (index == 1)
        {
            return ability1;
        }
        else if (index == 2)
        {
            return ability2;
        }
        else
        {
            return null;
        }
    }

    void OnUseAbility1(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            ability1.HandleInput();
        }
    }

    void OnUseAbility2(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            ability2.HandleInput();
        }
    }
}
