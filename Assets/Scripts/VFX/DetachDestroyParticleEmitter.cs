using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachDestroyParticleEmitter : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particles = null;
    [SerializeField]
    private float TimeToDestroy = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        particles.transform.SetParent(null);
    }

    void Update()
    {
        TimeToDestroy -= Time.deltaTime;
        if (TimeToDestroy < 0)
        {
            particles.Stop();
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            if (TimeToDestroy + 0.5f < 0)
            {
                Destroy(gameObject);
                Destroy(particles.gameObject);
            }
        }
    }
}
