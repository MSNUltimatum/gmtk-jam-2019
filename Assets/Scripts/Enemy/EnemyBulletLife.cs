using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletLife : MonoBehaviour
{
    [SerializeField]
    public float BulletSpeed = 12f;
    [SerializeField]
    protected float BulletLifeLength = 3f;
    [SerializeField]
    protected float ignoreCollisionTime = 0.35f;

    protected virtual void Update()
    {
        if (Pause.Paused) return;

        transform.Translate(Vector2.right * BulletSpeed * Time.deltaTime, Space.Self);
        ignoreCollisionTime -= Time.deltaTime;
        Destroy(gameObject, BulletLifeLength);
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (ignoreCollisionTime > 0) return;
        if (coll.gameObject.tag == "Environment")
        {
            Destroy(gameObject);
        }
        else if (coll.gameObject.tag == "Player")
        {
            CharacterLife life = coll.gameObject.GetComponent<CharacterLife>();
            life.Death();
        }
    }
}
