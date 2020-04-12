using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachDestroyParticleEmitter : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particles = null;
    [SerializeField]
    public float TimeToDestroy = 0.5f;
    [SerializeField]
    private float additionalTime = 0.5f;
    [SerializeField]
    private bool findAllInChildren = false;
    [SerializeField]
    private bool detachImmediately = true;
    
    void Start()
    {
        if (findAllInChildren)
        {
            allParticles = GetComponentsInChildren<ParticleSystem>();
            if (detachImmediately)
            {
                foreach (var particle in allParticles)
                {
                    particle.transform.SetParent(null);
                }
            }
        }
        else
        {
            if (detachImmediately) particles.transform.SetParent(null);
        }
    }

    void StopSpecificParticle()
    {
        if (!detachImmediately)
        {
            particles.transform.SetParent(null);
        }
        particles.Stop();
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    void StopAllParticles()
    {
        foreach (var particle in allParticles)
        {
            if (!detachImmediately)
            {
                particle.transform.SetParent(null);
            }
            particle.Stop();
        }
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer) spriteRenderer.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        TimeToDestroy -= Time.deltaTime;
        if (TimeToDestroy < 0)
        {
            if (!stopped)
            {
                stopped = true;
                if (findAllInChildren)
                {
                    StopAllParticles();
                }
                else
                {
                    StopSpecificParticle();
                }
            }

            if (TimeToDestroy + additionalTime < 0)
            {
                if (findAllInChildren)
                {
                    foreach (var particle in allParticles)
                    {
                        Destroy(particle.gameObject);
                    }
                }
                else
                {
                    Destroy(particles.gameObject);
                }
                Destroy(gameObject);
            }
        }
    }

    private bool stopped = false;
    private ParticleSystem[] allParticles = null;
}
