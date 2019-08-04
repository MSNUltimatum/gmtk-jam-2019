using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletLife : MonoBehaviour
{
    public float Speed = 0.5f;

    void FixedUpdate()
    {
        transform.Translate(Vector2.right * Speed);
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Destroy(coll.gameObject);
            Time.timeScale = 0;
        }

        if (coll.gameObject.tag == "Environment")
        {
            Destroy(gameObject);
        }
    }
}
