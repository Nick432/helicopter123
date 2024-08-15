using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    public void DisplayCanvas(bool state)
    {
        canvas.SetActive(state);
    }
}
