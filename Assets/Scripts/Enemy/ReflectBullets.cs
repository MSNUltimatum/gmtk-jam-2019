using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectBullets : MonoBehaviour
{
    [SerializeField] private GameObject bulletReflectAnim = null;
    [SerializeField] private GameObject bulletReflectedEffect = null;

    private void Start()
    {
        aiAgent = GetComponentInParent<AIAgent>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var bulletLife = coll.gameObject.GetComponent<BulletLife>();
        if (bulletLife)
        {
            RaycastHit2D hit = Physics2D.Raycast(coll.transform.position, coll.transform.right,
                float.PositiveInfinity, LayerMask.GetMask("Default"));
            if (hit)
            {
                Vector2 reflectDir = Vector2.Reflect(coll.transform.right, hit.normal);
                float rot = Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
                coll.transform.eulerAngles = new Vector3(0, 0, rot);
                bulletLife.KnockBack(aiAgent);
                TurnBulletIntoEnemy(bulletLife);
                var reflectionAnim = Instantiate(bulletReflectAnim, coll.transform.position, Quaternion.Euler(0, 0, rot - 90)).GetComponentInChildren<Animation>();
                reflectionAnim.wrapMode = WrapMode.Once;
                Instantiate(bulletReflectedEffect, bulletLife.transform);
            }
        }
    }

    private void TurnBulletIntoEnemy(BulletLife bullet)
    {
        bullet.piercing = true;
        // Make less collider size 
        var collider = bullet.GetComponent<BoxCollider2D>();
        collider.size *= 0.75f;

        var enemyBullet = bullet.gameObject.AddComponent<EnemyBulletLife>();
        enemyBullet.BulletSpeed = 0;
        enemyBullet.BulletLifeLength = Mathf.Infinity;
        enemyBullet.ignoreCollisionTime = 0;
    }

    private AIAgent aiAgent = null;
}
