using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExplosiveBulletLife : BulletLife
{
    [SerializeField]
    private float radius = 2f;
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(coll.transform.position, radius);
            var enemys = (from t in collider2Ds
                               where t.transform.gameObject.tag == "Enemy"
                               select t).ToArray();
            foreach(var i in enemys)
            {
                var monsterLife = i.gameObject.GetComponent<MonsterLife>();
                if (monsterLife && !isKilled)
                {
                    monsterLife.Damage(gameObject);
                    Wave(i.GetComponent<Rigidbody2D>(), 8);
                    isKilled = true;
                }
                else if(!monsterLife)
                {
                    Debug.LogError("ОШИБКА: УСТАНОВИТЕ МОНСТРУ " + coll.gameObject.name + " КОМПОНЕНТ MonsterLife");
                    Destroy(coll.gameObject);
                }
                else
                {
                    Wave(i.GetComponent<Rigidbody2D>(), 8);
                }
            }
            DestroyBullet();
        }
        else if (coll.gameObject.tag == "Environment")
        {
            if (coll.gameObject.GetComponent<DestructibleWall>() != null)
            {
                DestroyBullet();
            }
            if (coll.gameObject.GetComponent<MirrorWall>() != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right,
                    float.PositiveInfinity, LayerMask.GetMask("Default"));
                if (hit)
                {
                    Vector2 reflectDir = Vector2.Reflect(transform.right, hit.normal);
                    float rot = Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(0, 0, rot);
                }
            }
            else
            {
                DestroyBullet();
            }
        }
    }

    private void Wave(Rigidbody2D Enemy, float thrust)
    {
        Enemy.drag = 1;
        Vector2 direction = Enemy.transform.position - transform.position;
        direction = direction.normalized * thrust;
        Enemy.AddForce(direction, ForceMode2D.Impulse);

        var moveComp = Enemy.GetComponent<AIAgent>();
        if (moveComp)
        {
            moveComp.StopMovement(0.7f);
        }
        else
        {
            Debug.LogWarning("No Move Component on enemy? Is it ok?");
        }
    }
    bool isKilled = false;
}
