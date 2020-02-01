using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Chapter1MirrorBulletInfuser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<BulletLife>() != null)
        {
            coll.gameObject.AddComponent<Chapter1BossInfusedBullet>();
            coll.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
            var emitter = coll.GetComponentInChildren<ParticleSystem>().main;
            emitter.startColor = Color.cyan;
            coll.gameObject.GetComponent<Light2D>().color = Color.cyan;
        }
    }
}
