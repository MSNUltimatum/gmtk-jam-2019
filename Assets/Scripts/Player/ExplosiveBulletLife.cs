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
                    Wave(i.GetComponent<AIAgent>(), 8);
                    isKilled = true;
                }
                else
                {
                    Wave(i.GetComponent<AIAgent>(), 8);
                }
            }
        }
    }

    protected virtual void Wave(AIAgent enemy, float thrust)
    {
        Vector2 direction = enemy.transform.position - transform.position;
        direction = direction.normalized * thrust;
        enemy.velocity += direction * Time.deltaTime;
    }

    bool isKilled = false;
}
