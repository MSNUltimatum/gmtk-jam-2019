using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExplosiveBulletLife : BulletLife
{
    [SerializeField]
    private float explosionRadius = 2f;
    protected override void EnemyCollider(Collider2D coll)
    {
        FindMonsters(coll);
        DestroyBullet();
    }

    protected void FindMonsters(Collider2D coll)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(coll.transform.position, explosionRadius);
        var enemys = (from t in collider2Ds
                      where t.transform.gameObject.tag == "Enemy"
                      select t).ToArray();
        foreach (var i in enemys)
        {
            var monsterLife = i.gameObject.GetComponent<MonsterLife>();
            if (monsterLife)
            {
                var tmp = monsterLife.HP;
                if (!isKilled)
                    DamageMonster(monsterLife);

                if (!isKilled && monsterLife.HP < tmp)
                {
                    Wave(i.GetComponent<Rigidbody2D>(), 8);
                    isKilled = true;
                }
                else
                {
                    Wave(i.GetComponent<Rigidbody2D>(), 8);
                }
            }
        }
    }

    protected virtual void Wave(Rigidbody2D Enemy, float thrust)
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
