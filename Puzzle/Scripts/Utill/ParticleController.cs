using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    public float multiplier = 1;
    ParticleSystem[] Particles;

    private void Start()
    {
        Particles = GetComponentsInChildren<ParticleSystem>();
        Stop();
    }

    public void Play()
    {       
        if(IsInvoking("Stop"))
            CancelInvoke("Stop");

        foreach (ParticleSystem particle in Particles)
        {
            particle.gameObject.SetActive(true);

            ParticleSystem.MainModule mainModule = particle.main;
            mainModule.startSizeMultiplier *= multiplier;
            mainModule.startSpeedMultiplier *= multiplier;
            mainModule.startLifetimeMultiplier *= Mathf.Lerp(multiplier, 1, 0.5f);
            particle.Clear();
            particle.Play();
        }

        Invoke("Stop", 2);
    }

    public void Stop()
    {
        foreach (ParticleSystem particle in Particles)
        {
            particle.gameObject.SetActive(false);
        }
    }
}
