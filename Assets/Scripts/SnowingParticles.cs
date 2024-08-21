using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowingParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;

    float originalEmission;

    void Start()
    {
        var particlesEmission = particles.emission;
        particlesEmission.enabled = false;
    }

    public void SetSnowing(bool state)
    {
        var particlesEmission = particles.emission;
        particlesEmission.enabled = state;
    }
}
