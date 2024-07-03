using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave", fileName = "New Wave")]

public class WaveSO : ScriptableObject
{
    public float timeUntilWaveStarts = 0f;

    public List<WaveSOElement> waveElements = new List<WaveSOElement>();

}
