using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLife : MonoBehaviour
{
    public float Speed = 0.05f;

    void FixedUpdate()
    { 
        transform.Translate(Vector2.right * Speed);
        Destroy(gameObject,1.2f); 
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            Destroy(coll.gameObject);
            Destroy(gameObject);
        }

        if (coll.gameObject.tag == "Environment")
        {
            Destroy(gameObject);
        }
    }
}
