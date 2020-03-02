using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "ExplosiveBulletMod", menuName = "ScriptableObject/BulletModifier/ExplosiveBulletMod", order = 1)]
public class ExplosiveBulletMod : BulletModifier
{
    [SerializeField]
    private float explosionRadius = 2f;

    bool isKilled = false;

    public override void HitEnemyModifier(BulletLife bullet, Collider2D coll)
    {
        base.HitEnemyModifier(bullet, coll);
        ModEffect(bullet, coll);
    }

    public override void HitEnvironmentModifier(BulletLife bullet, Collider2D coll)
    {
        base.HitEnvironmentModifier(bullet, coll);
        ModEffect(bullet, coll);
    }

    protected void ModEffect(BulletLife bullet, Collider2D coll)
    {
        var monsters = FindMonsters(coll);
        ExplosiveWave(monsters, bullet);
    }

    protected Collider2D[] FindMonsters(Collider2D coll)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(coll.transform.position, explosionRadius);
        var enemys = (from t in collider2Ds
                      where t.transform.gameObject.tag == "Enemy"
                      select t).ToArray();
        return enemys;
    }

    protected virtual void ExplosiveWave(Collider2D[] enemys, BulletLife bullet)
    {
        foreach (var i in enemys)
        {
            var monsterLife = i.gameObject.GetComponent<MonsterLife>();
            if (monsterLife)
            {
                var tmp = monsterLife.HP;
                if (!isKilled)
                    bullet.DamageMonster(monsterLife, 0.5f);

                if (!isKilled && monsterLife.HP < tmp)
                {
                    bullet.DamageMonster(monsterLife, 0);
                    isKilled = true;
                }
                else
                {
                    bullet.DamageMonster(monsterLife, 0);
                }
            }
        }
    }

    protected virtual void Push(AIAgent enemy, float pushPower, Vector3 from)
    {
        Vector2 direction = enemy.transform.position - from;
        direction = direction.normalized * pushPower;
        enemy.velocity += direction;
    }
}
